Imports System.Text
Imports System.Windows.Media.Media3D

Namespace Particles
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Public Class ParticleSystemManager
		Private particleSystems As Dictionary(Of Color, ParticleSystem)

		Public Sub New()
			Me.particleSystems = New Dictionary(Of Color, ParticleSystem)()
		End Sub

		Public Sub Update(ByVal elapsed As Single)
			For Each ps As ParticleSystem In Me.particleSystems.Values
				ps.Update(elapsed)
			Next ps
		End Sub

		Public Sub SpawnParticle(ByVal position As Point3D, ByVal speed As Double, ByVal color As Color, ByVal size As Double, ByVal life As Double)
			Try
				Dim ps As ParticleSystem = Me.particleSystems(color)
				ps.SpawnParticle(position, speed, size, life)
			Catch
			End Try
		End Sub

		Public Function CreateParticleSystem(ByVal maxCount As Integer, ByVal color As Color) As Model3D
			Dim ps As New ParticleSystem(maxCount, color)
			Me.particleSystems.Add(color, ps)
			Return ps.ParticleModel
		End Function

		Public ReadOnly Property ActiveParticleCount() As Integer
			Get
				Dim count As Integer = 0
				For Each ps As ParticleSystem In Me.particleSystems.Values
					count += ps.Count
				Next ps
				Return count
			End Get
		End Property
	End Class

	Public Class ParticleSystem
		Private particleList As List(Of Particle)
'INSTANT VB NOTE: The variable particleModel was renamed since Visual Basic does not allow class members with the same name:
		Private particleModel_Renamed As GeometryModel3D
'INSTANT VB NOTE: The variable maxParticleCount was renamed since Visual Basic does not allow class members with the same name:
		Private maxParticleCount_Renamed As Integer
		Private rand As Random

		Public Sub New(ByVal maxCount As Integer, ByVal color As Color)
			Me.maxParticleCount_Renamed = maxCount

			Me.particleList = New List(Of Particle)()

			Me.particleModel_Renamed = New GeometryModel3D()
			Me.particleModel_Renamed.Geometry = New MeshGeometry3D()

			Dim e As New Ellipse()
			e.Width = 32.0
			e.Height = 32.0
			Dim b As New RadialGradientBrush()
			b.GradientStops.Add(New GradientStop(Color.FromArgb(&HFF, color.R, color.G, color.B), 0.25))
			b.GradientStops.Add(New GradientStop(Color.FromArgb(&H00, color.R, color.G, color.B), 1.0))
			e.Fill = b
			e.Measure(New Size(32, 32))
			e.Arrange(New Rect(0, 0, 32, 32))

			Dim brush As Brush = Nothing

#If USE_VISUALBRUSH Then
			brush = New VisualBrush(e)
#Else
			Dim renderTarget As New RenderTargetBitmap(32, 32, 96, 96, PixelFormats.Pbgra32)
			renderTarget.Render(e)
			renderTarget.Freeze()
			brush = New ImageBrush(renderTarget)
#End If

			Dim material As New DiffuseMaterial(brush)

			Me.particleModel_Renamed.Material = material

			Me.rand = New Random(brush.GetHashCode())
		End Sub

		Public Sub Update(ByVal elapsed As Double)
			Dim deadList As New List(Of Particle)()

			' Update all particles
			For Each p As Particle In Me.particleList
				p.Position += p.Velocity * elapsed
				p.Life -= p.Decay * elapsed
				p.Size = p.StartSize * (p.Life / p.StartLife)
				If p.Life <= 0.0 Then
					deadList.Add(p)
				End If
			Next p

			For Each p As Particle In deadList
				Me.particleList.Remove(p)
			Next p

			UpdateGeometry()
		End Sub

		Private Sub UpdateGeometry()
			Dim positions As New Point3DCollection()
			Dim indices As New Int32Collection()
			Dim texcoords As New PointCollection()

			For i As Integer = 0 To Me.particleList.Count - 1
				Dim positionIndex As Integer = i * 4
				Dim indexIndex As Integer = i * 6
				Dim p As Particle = Me.particleList(i)

				Dim p1 As New Point3D(p.Position.X, p.Position.Y, p.Position.Z)
				Dim p2 As New Point3D(p.Position.X, p.Position.Y + p.Size, p.Position.Z)
				Dim p3 As New Point3D(p.Position.X + p.Size, p.Position.Y + p.Size, p.Position.Z)
				Dim p4 As New Point3D(p.Position.X + p.Size, p.Position.Y, p.Position.Z)

				positions.Add(p1)
				positions.Add(p2)
				positions.Add(p3)
				positions.Add(p4)

				Dim t1 As New Point(0.0, 0.0)
				Dim t2 As New Point(0.0, 1.0)
				Dim t3 As New Point(1.0, 1.0)
				Dim t4 As New Point(1.0, 0.0)

				texcoords.Add(t1)
				texcoords.Add(t2)
				texcoords.Add(t3)
				texcoords.Add(t4)

				indices.Add(positionIndex)
				indices.Add(positionIndex+2)
				indices.Add(positionIndex+1)
				indices.Add(positionIndex)
				indices.Add(positionIndex+3)
				indices.Add(positionIndex+2)
			Next i

			CType(Me.particleModel_Renamed.Geometry, MeshGeometry3D).Positions = positions
			CType(Me.particleModel_Renamed.Geometry, MeshGeometry3D).TriangleIndices = indices
			CType(Me.particleModel_Renamed.Geometry, MeshGeometry3D).TextureCoordinates = texcoords

		End Sub

		Public Sub SpawnParticle(ByVal position As Point3D, ByVal speed As Double, ByVal size As Double, ByVal life As Double)
			If Me.particleList.Count > Me.maxParticleCount_Renamed Then
				Return
			End If
			Dim p As New Particle()
			p.Position = position
			p.StartLife = life
			p.Life = life
			p.StartSize = size
			p.Size = size

			Dim x As Single = 1.0f - CSng(rand.NextDouble()) * 2.0f
			Dim z As Single = 1.0f - CSng(rand.NextDouble()) * 2.0f

			Dim v As New Vector3D(x, z, 0.0)
			v.Normalize()
			v *= (CSng(rand.NextDouble()) + 0.25f) * CSng(speed)

			p.Velocity = New Vector3D(v.X, v.Y, v.Z)

            p.Decay = 1.0F

			Me.particleList.Add(p)
		End Sub

		Public Property MaxParticleCount() As Integer
			Get
				Return Me.maxParticleCount_Renamed
			End Get
			Set(ByVal value As Integer)
				Me.maxParticleCount_Renamed = value
			End Set
		End Property

		Public ReadOnly Property Count() As Integer
			Get
				Return Me.particleList.Count
			End Get
		End Property

		Public ReadOnly Property ParticleModel() As Model3D
			Get
				Return Me.particleModel_Renamed
			End Get
		End Property
	End Class


	Public Class Particle
		Public Position As Point3D
		Public Velocity As Vector3D
		Public StartLife As Double
		Public Life As Double
		Public Decay As Double
		Public StartSize As Double
		Public Size As Double
	End Class

	Partial Public Class MainWindow
		Inherits Window
        Private frameTimer As System.Windows.Threading.DispatcherTimer

		Private spawnPoint As Point3D
		Private elapsed As Double
		Private totalElapsed As Double
		Private lastTick As Integer
		Private currentTick As Integer

		Private frameCount As Integer
		Private frameCountTime As Double
		Private frameRate As Integer

		Private pm As ParticleSystemManager

		Private rand As Random

		Public Sub New()
			InitializeComponent()

            frameTimer = New System.Windows.Threading.DispatcherTimer()
			AddHandler frameTimer.Tick, AddressOf OnFrame
			frameTimer.Interval = TimeSpan.FromSeconds(1.0 / 60.0)
			frameTimer.Start()

			Me.spawnPoint = New Point3D(0.0, 0.0, 0.0)
			Me.lastTick = Environment.TickCount

			pm = New ParticleSystemManager()

			Me.WorldModels.Children.Add(pm.CreateParticleSystem(1000, Colors.Gray))
			Me.WorldModels.Children.Add(pm.CreateParticleSystem(1000, Colors.Red))
			Me.WorldModels.Children.Add(pm.CreateParticleSystem(1000, Colors.Silver))
			Me.WorldModels.Children.Add(pm.CreateParticleSystem(1000, Colors.Orange))
			Me.WorldModels.Children.Add(pm.CreateParticleSystem(1000, Colors.Yellow))

			rand = New Random(Me.GetHashCode())

			AddHandler KeyDown, AddressOf Window1_KeyDown
			Me.Cursor = Cursors.None
		End Sub

		Private Sub Window1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Key = Key.Escape Then
				Me.Close()
			End If
		End Sub

		Private Sub OnFrame(ByVal sender As Object, ByVal e As EventArgs)
            ' Calculate frame time
			Me.currentTick = Environment.TickCount
			Me.elapsed = CDbl(Me.currentTick - Me.lastTick) / 1000.0
			Me.totalElapsed += Me.elapsed
			Me.lastTick = Me.currentTick

			frameCount += 1
			frameCountTime += elapsed
			If frameCountTime >= 1.0 Then
				frameCountTime -= 1.0
				frameRate = frameCount
				frameCount = 0
				Me.FrameRateLabel.Content = "FPS: " & frameRate.ToString() & "  Particles: " & pm.ActiveParticleCount.ToString()
			End If

			pm.Update(CSng(elapsed))
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Red, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Orange, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Silver, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Gray, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Red, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Orange, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Silver, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Gray, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Red, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Yellow, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Silver, rand.NextDouble(), 2.5 * rand.NextDouble())
			pm.SpawnParticle(Me.spawnPoint, 10.0, Colors.Yellow, rand.NextDouble(), 2.5 * rand.NextDouble())

			Dim c As Double = Math.Cos(Me.totalElapsed)
			Dim s As Double = Math.Sin(Me.totalElapsed)

			Me.spawnPoint = New Point3D(s * 32.0, c * 32.0, 0.0)
		End Sub

	End Class
End Namespace