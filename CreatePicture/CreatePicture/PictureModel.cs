using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Reflection;

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
            // 先构造要编译的源代码（和原来相同）
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

            // 使用 Roslyn 在内存中编译
            var syntaxTree = CSharpSyntaxTree.ParseText(Code);

            // 准备引用：至少需要核心库、定义 ICreator 的程序集、以及入口程序集
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICreator).Assembly.Location),
            };

            // 如果需要其它系统库（例如 System.Xml、System.Data），可以按需添加：
            // MetadataReference.CreateFromFile(typeof(System.Xml.XmlDocument).Assembly.Location)

            var compilation = CSharpCompilation.Create(
                assemblyName: $"InMemory_{Guid.NewGuid():N}",
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            ICreator aCreator;

            if (!emitResult.Success)
            {
                // 收集错误并抛出，保持原有行为（包含源代码上下文）
                string[] aLines = Code.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                var sb = new System.Text.StringBuilder();
                sb.AppendLine(Code);

                foreach (var diag in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                {
                    var loc = diag.Location;
                    int line = 0;
                    if (loc.IsInSource)
                    {
                        var lineSpan = loc.GetLineSpan();
                        line = lineSpan.StartLinePosition.Line + 1;
                        if (line >= 1 && line <= aLines.Length)
                            sb.AppendLine($"【{aLines[line - 1]}】：{diag.GetMessage()}");
                        else
                            sb.AppendLine($"【line {line}】:{diag.GetMessage()}");
                    }
                    else
                    {
                        sb.AppendLine(diag.ToString());
                    }
                }
                throw new ApplicationException(sb.ToString());
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                // 找到第一个导出的类型并创建实例（保持与原实现一致）
                Type aType = assembly.GetExportedTypes().First();
                aCreator = assembly.CreateInstance(aType.FullName) as ICreator;
            }

            // 生成图像（原有代码保持不变）
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
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
