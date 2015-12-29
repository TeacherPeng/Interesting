using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Math;

namespace CreatePicture
{
    class PictureModel : INotifyPropertyChanged
    {
        public int Width { get { return _Width; } set { if (_Width == value) return;  _Width = value;  OnPropertyChanged(nameof(Width)); } }
        private int _Width = 1024;

        public int Height { get { return _Height; } set { if (_Height == value) return;  _Height = value;  OnPropertyChanged(nameof(Height)); } }
        private int _Height = 1024;

        public string C { get { return _C; }set { if (_C == value) return; _C = value; OnPropertyChanged(nameof(C)); } }
        private string _C = @"private const int C = 256;
private Random _random = new Random();
private int rand() { return _random.Next(); }
private int rand(int n) { return _random.Next(n); }
private int rand(int s, int t) {return _random.Next(s, t); }
private double randdbl() { return _random.NextDouble(); }
private double _sq(double x) { return x * x; }
private double _cb(double x) { return Math.Abs(x * x * x); }
private double _cr(double x) { return Math.Pow(x, 1.0/3.0); }
private double sin(double x) { return Math.Sin(x); }
private double cos(double x) { return Math.Cos(x); }
private double atan2(double x, double y) { return Math.Atan2(x, y); }
private double acos(double x) {return Math.Acos(x); }";

        public string R { get { return _R; } set { if (_R == value) return; _R = value; OnPropertyChanged(nameof(R)); } }
        private string _R = "return rand(C);";
        public string G { get { return _G; } set { if (_G == value) return; _G = value; OnPropertyChanged(nameof(G)); } }

        private string _G = "return rand(C);";
        public string B { get { return _B; } set { if (_B == value) return; _B = value; OnPropertyChanged(nameof(B)); } }
        private string _B = "return rand(C);";

        public BitmapSource Picture { get { return _Picture; } set { if (_Picture == value) return; _Picture = value; OnPropertyChanged(nameof(Picture)); } }

        private BitmapSource _Picture;

        private readonly PixelFormat _Format = PixelFormats.Bgr24;

        public void Exec()
        {
            // 编译代码
            Dictionary<string, string> aProviderOptions = new Dictionary<string, string>();
            aProviderOptions.Add("CompilerVersion", "v3.5");
            CSharpCodeProvider aCodeDomProvider = new CSharpCodeProvider(aProviderOptions);
            CompilerParameters aCompilerParameters = new CompilerParameters();
            aCompilerParameters.ReferencedAssemblies.Add("System.dll");
            aCompilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            aCompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            aCompilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            aCompilerParameters.ReferencedAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(ICreator)).ManifestModule.Name);
            aCompilerParameters.ReferencedAssemblies.Add(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            aCompilerParameters.GenerateExecutable = false;
            aCompilerParameters.GenerateInMemory = true;

            string Code = $@"using System;
namespace CreatePicture
{{
    public class Creator : ICreator
    {{
        private const int W = {Width};
        private const int H = {Height};
{C}
        public int B(int x, int y)
        {{
{B}
        }}

        public int G(int x, int y)
        {{
{G}
        }}

        public int R(int x, int y)
        {{
{R}
        }}
    }}
}}
";
            CompilerResults aCompilerResults = aCodeDomProvider.CompileAssemblyFromSource(aCompilerParameters, Code);
            ICreator aCreator;
            if (aCompilerResults.Errors.Count > 0)
            {
                string[] aLines = Code.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                System.Text.StringBuilder aErrorTextBuilder = new System.Text.StringBuilder();
                foreach (CompilerError aCompilerError in aCompilerResults.Errors)
                {
                    aErrorTextBuilder.AppendLine($"【{aLines[aCompilerError.Line - 1]}】：{aCompilerError.ErrorText}");
                }
                throw new System.ApplicationException(aErrorTextBuilder.ToString());
            }
            else
            {
                Type[] aTypes = aCompilerResults.CompiledAssembly.GetExportedTypes();
                Type aType = aTypes[0];
                aCreator = aCompilerResults.CompiledAssembly.CreateInstance(aType.FullName) as ICreator;
            }

            // 生成图像
            int aStride = (_Format.BitsPerPixel * Width + 7) / 8;
            byte[] aPixels = new byte[aStride * Height];
            int aRowIndex = 0;
            for (int y = 0; y < Height; y++)
            {
                int i = aRowIndex;
                for (int x = 0; x < Width; x++)
                {
                    aPixels[i++] = (byte)aCreator.B(x, y);
                    aPixels[i++] = (byte)aCreator.G(x, y);
                    aPixels[i++] = (byte)aCreator.R(x, y);
                }
                aRowIndex += aStride;
            }
            Picture = BitmapImage.Create(Width, Height, 96, 96, _Format, null, aPixels, aStride);
        }

        internal void LoadCode(string aFileName)
        {
            XDocument aXDocument = XDocument.Load(aFileName);
            Width = int.Parse(aXDocument.Root.Attribute("Width").Value);
            Height = int.Parse(aXDocument.Root.Attribute("Height").Value);
            C = aXDocument.Root.Element(nameof(C)).Value;
            R = aXDocument.Root.Element(nameof(R)).Value;
            G = aXDocument.Root.Element(nameof(G)).Value;
            B = aXDocument.Root.Element(nameof(B)).Value;
        }

        internal void SaveCode(string aFileName)
        {
            XDocument aXDocument = new XDocument(new XElement("Code",
                new XAttribute(nameof(Width), Width),
                new XAttribute(nameof(Height), Height),
                new XElement(nameof(C), new XCData(C)),
                new XElement(nameof(R), new XCData(R)),
                new XElement(nameof(G), new XCData(G)),
                new XElement(nameof(B), new XCData(B))));
            aXDocument.Save(aFileName);
        }

        internal void SavePicture(string aFileName)
        {
            JpegBitmapEncoder aEncoder = new JpegBitmapEncoder();
            aEncoder.Frames.Add(BitmapFrame.Create(Picture));
            using (FileStream aStream = new FileStream(aFileName, FileMode.CreateNew))
            {
                aEncoder.Save(aStream);
                aStream.Close();
            }
        }

        private void OnPropertyChanged(string aPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
