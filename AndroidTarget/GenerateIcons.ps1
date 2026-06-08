Add-Type -AssemblyName System.Drawing

$sourcePath = "icon_source.png"
$baseDir = "Resources"

$sizes = @(
    @{ name = "mipmap-mdpi"; size = 48 },
    @{ name = "mipmap-hdpi"; size = 72 },
    @{ name = "mipmap-xhdpi"; size = 96 },
    @{ name = "mipmap-xxhdpi"; size = 144 },
    @{ name = "mipmap-xxxhdpi"; size = 192 }
)

if (-not (Test-Path $sourcePath)) {
    Write-Host "错误：未找到源图片文件 $sourcePath" -ForegroundColor Red
    exit 1
}

try {
    $sourceImage = [System.Drawing.Image]::FromFile($sourcePath)
    
    foreach ($item in $sizes) {
        $outputDir = Join-Path $baseDir $item.name
        $outputPath = Join-Path $outputDir "appicon.png"
        $foregroundPath = Join-Path $outputDir "appicon_foreground.png"
        
        if (-not (Test-Path $outputDir)) {
            New-Item -ItemType Directory -Path $outputDir | Out-Null
        }
        
        $newImage = new-object System.Drawing.Bitmap($item.size, $item.size)
        $graphics = [System.Drawing.Graphics]::FromImage($newImage)
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.DrawImage($sourceImage, 0, 0, $item.size, $item.size)
        $newImage.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
        
        # 复制为 appicon_foreground.png（覆盖同名文件）
        Copy-Item -Path $outputPath -Destination $foregroundPath -Force

        $graphics.Dispose()
        $newImage.Dispose()
        
        Write-Host "已生成: $outputPath" -ForegroundColor Green
        Write-Host "已复制为: $foregroundPath" -ForegroundColor Yellow
    }
    
    $sourceImage.Dispose()
    Write-Host "`n图标生成完成！" -ForegroundColor Cyan
}
catch {
    Write-Host "生成图标时发生错误: $_" -ForegroundColor Red
}
