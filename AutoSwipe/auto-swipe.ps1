$env:path += ';C:\Program Files (x86)\Android\android-sdk\platform-tools;'
adb shell monkey -p com.ss.android.ugc.aweme.lite -c android.intent.category.LAUNCHER 1
for(;;)
{
$startx = Get-Random -Minimum 0 -Maximum 100
$startx += 450
$starty = Get-Random -Minimum 0 -Maximum 100
$starty += 1400
$endx = Get-Random -Minimum 0 -Maximum 100
$endx += 450
$endy = Get-Random -Minimum 0 -Maximum 100
$endy += 150
$interval = Get-Random -Minimum 50 -Maximum 300
adb shell input swipe $startx $starty $endx $endy $interval
$interval = Get-Random -Minimum 3 -Maximum 10
Write-Host "Swip after $interval seconds..."
Start-Sleep -Seconds $interval
}
