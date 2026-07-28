::[Bat To Exe Converter]
::
::YAwzoRdxOk+EWAjk
::fBw5plQjdCyDJGyX8VAjFDV9byuuFV+fNYk45//14+WGpl4hVuwrcYzU1PqHI+9z
::YAwzuBVtJxjWCl3EqQJgSA==
::ZR4luwNxJguZRRnk
::Yhs/ulQjdF+5
::cxAkpRVqdFKZSDk=
::cBs/ulQjdF+5
::ZR41oxFsdFKZSDk=
::eBoioBt6dFKZSDk=
::cRo6pxp7LAbNWATEpCI=
::egkzugNsPRvcWATEpCI=
::dAsiuh18IRvcCxnZtBJQ
::cRYluBh/LU+EWAnk
::YxY4rhs+aU+IeA==
::cxY6rQJ7JhzQF1fEqQJmZks0
::ZQ05rAF9IBncCkqN+0xwdVsCAlTi
::ZQ05rAF9IAHYFVzEqQISBi8ZbQqGLmSzAvgo5+f3/Io=
::eg0/rx1wNQPfEVWB+kM9LVsJDCasCCabCLEO5+H/ot6IrUEONA==
::fBEirQZwNQPfEVWB+kM9LVsJDGQ=
::cRolqwZ3JBvQF1fEqQJQ
::dhA7uBVwLU+EWFuK4FU/OgNAWkqBM2ba
::YQ03rBFzNR3SWATElA==
::dhAmsQZ3MwfNWATE9kwkPxRGVBCUcmi1C9U=
::ZQ0/vhVqMQ3MEVWAtB9wSA==
::Zg8zqx1/OA3MEVWAtB9wSA==
::dhA7pRFwIByZRRnk
::Zh4grVQjdCyDJGyX8VAjFDV9byuuFV+fNYk47fvw++WXnm8zYK8edovJ1b2KH9Qc5Un3O5M10xo=
::YB416Ek+ZG8=
::
::
::978f952a14a936cc963da21a135fa983
@echo off
title BNT Android Tools Dashboard v8.11
color 0A
mode con: cols=90 lines=55
setlocal EnableDelayedExpansion

if not exist "%TEMP%\bnt" mkdir "%TEMP%\bnt"
set "LOG=%TEMP%\bnt\tool_log.txt"
echo [%date% %time%] BNT Android Tools v8.11 started >> "%LOG%"

cls
echo.
echo      ____  _____ _   _ ______   ____  ____  ____
echo     ^| __ )^| ____^| \ ^| ^|  _ \ \ / /  \/  \^|  _ \
echo     ^|  _ \^|  _^| ^|  \^| ^| ^| ^| \ V /^| ^|\^/^| ^| ^|_)^|
echo     ^| ^|_) ^| ^|___^| ^|\  ^| ^| ^| ^| ^| ^| ^|  ^| ^|  _ <
echo     ^|____/^|_____^|_^| \_^|_^|_^|_^| \_/ ^|_^|  ^|_^|_^| \_\
echo.
echo      ____  _   _  ___  ____  ___  __  __    _    _
echo     ^|  _ \^| ^| ^| ^|/ _ \/ __^|/ _ \^|  \/  ^|  / \  ^| ^|
echo     ^| ^|_) ^| ^| ^| ^| ^| ^|__\__ \^| ^| ^| ^|  \^/^| ^| / _ \ ^| ^|
echo     ^|  __/^| ^|_^| ^|  _^<^|__ /^| ^|_^| ^| ^|  ^| ^|/ ___ \^| ^|___
echo     ^|_^|    \___/^|_^| ^|___/ \___/^|_^|  ^|_^|/_/   \_\_____[
echo.
echo      ____  _____  _____  __  __  ___   ____  ___
echo     ^|  _ \^|  ___^|^|_   _^|^|  \/  ^|/ _ \ / ___^|/ _ \
echo     ^| ^|_) ^| ^|_    ^| ^|  ^| ^|\^/^| ^| ^| ^| ^| ^|  _^| ^| ^| ^|
echo     ^|  __/^|  _|   ^| ^|  ^| ^|  ^| ^| ^|_^| ^| ^|_^| ^| ^|_^| ^|
echo     ^|_^|   ^|_^|    ^|_^|  ^|_^|  ^|_^|\___/ \____/ \___/
echo.
echo     [AD REMOVAL] [FRP BYPASS] [BLOATWARE] [UTILITIES]
echo     [PRIVACY] [APP MANAGER] [QUICK ACTIONS] [DEVELOPER]
echo     [NETWORK] [SETTINGS]
echo.
echo     Created by BNTWORX ^| v8.11
echo.

echo   [INIT] Checking ADB...
adb version >nul 2>&1
if errorlevel 1 (
    color 0C
    echo   [FATAL] ADB not found! Install from:
    echo   https://developer.android.com/tools/releases/platform-tools
    pause
    exit /b 1
)
echo   [OK] ADB found.
echo.

:device_retry
echo   [INIT] Scanning devices...
adb devices > "%TEMP%\bnt\devices.txt" 2>nul
set /a DEVICE_COUNT=0
for /f "skip=1 tokens=1,2" %%A in ('adb devices') do (
    if "%%B"=="device" set /a DEVICE_COUNT+=1
)
if !DEVICE_COUNT! EQU 0 (
    color 0C
    echo   [ERROR] No device found!
    echo   1. Enable USB Debugging   2. Connect USB
    echo   3. Accept RSA prompt      4. Try different cable
    pause >nul
    goto :device_retry
)
echo   [OK] !DEVICE_COUNT! device connected.
echo.

echo   [INIT] Reading device info...
for /f "delims=" %%A in ('adb shell getprop ro.product.manufacturer 2^>nul') do set "MFG=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.product.model 2^>nul') do set "MODEL=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.build.version.release 2^>nul') do set "ANDROID=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.build.version.sdk 2^>nul') do set "SDK=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.build.display.id 2^>nul') do set "BUILD=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.product.cpu.abi 2^>nul') do set "ARCH=%%A"
for /f "delims=" %%A in ('adb shell settings get global device_name 2^>nul') do set "DEVNAME=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.product.brand 2^>nul') do set "BRAND=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.product.board 2^>nul') do set "BOARD=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.build.version.security_patch 2^>nul') do set "SECPATCH=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.product.device 2^>nul') do set "DEVICE=%%A"
for /f "delims=" %%A in ('adb shell getprop ro.build.type 2^>nul') do set "BUILDTYPE=%%A"
for %%V in (MFG MODEL ANDROID SDK BUILD ARCH DEVNAME BRAND BOARD SECPATCH DEVICE BUILDTYPE) do (
    if defined %%V (
        set "_val=!%%V!"
        for /f "tokens=* delims= " %%Z in ("!_val!") do set "%%V=%%Z"
    )
)

echo   [INIT] Checking root...
adb shell su -c "id" 2>nul | findstr "uid=0" >nul 2>&1
if errorlevel 1 (
    set "ROOTED=0"
    set "ROOT_STAT=[NO ROOT]"
) else (
    set "ROOTED=1"
    set "ROOT_STAT=[ROOTED]"
)
echo   [OK] Root: !ROOT_STAT!
echo.
echo   =======================================================================
echo   [READY] Launching Dashboard...
echo   =======================================================================
timeout /t 1 >nul

:main_menu
cls
echo.
echo   =======================================================================
echo   ^|             BNT ANDROID TOOLS DASHBOARD v8.11                        ^|
echo   ^|                 CREATED BY BNTWORX                                  ^|
echo   =======================================================================
echo.
echo   +--------------------------------------------------------------------+
echo   ^| DEVICE: !DEVNAME! ^| !BRAND! !MODEL! ^| Android !ANDROID! ^(SDK !SDK!^)    ^|
echo   ^| BOARD: !BOARD! ^| CPU: !ARCH! ^| Patch: !SECPATCH!       ^|
echo   ^| Root: !ROOT_STAT!                                              ^|
echo   +--------------------------------------------------------------------+
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [1] AD REMOVAL        ^|  ^|      [2] FRP BYPASS        ^|
echo   ^|   Hosts, DNS, nuclear      ^|  ^|   Setup, accounts, FRP    ^|
echo   ^|   Disable SDKs, banners    ^|  ^|   Full bypass suite        ^|
echo   +----------------------------+  +----------------------------+
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [3] BLOATWARE         ^|  ^|      [4] DEVICE UTILS      ^|
echo   ^|   13 brands, full clean    ^|  ^|   Info, reboot, backup     ^|
echo   ^|   Search, reinstall        ^|  ^|   Screenshot, APK install  ^|
echo   +----------------------------+  +----------------------------+
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [5] PRIVACY SHIELD    ^|  ^|      [6] APP MANAGER       ^|
echo   ^|   Permissions, telemetry   ^|  ^|   Force stop, clear, bulk  ^|
echo   ^|   Audit, encrypt           ^|  ^|   Uninstall, info          ^|
echo   +----------------------------+  +----------------------------+
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [7] QUICK ACTIONS     ^|  ^|      [8] DEVELOPER TOOLS   ^|
echo   ^|   Optimize, cache clear    ^|  ^|   Logcat, dumpsys, shell   ^|
echo   ^|   Timeout, USB, battery    ^|  ^|   Monkey test, benchmark   ^|
echo   +----------------------------+  +----------------------------+
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [9] NETWORK TOOLS     ^|  ^|      [0] SETTINGS          ^|
echo   ^|   WiFi, DNS, ping, IP      ^|  ^|   About, logs, export      ^|
echo   +----------------------------+  +----------------------------+
echo.
echo   -----------------------------------------------------------------------
echo   [E] Exit Dashboard
echo   -----------------------------------------------------------------------
echo.
set /p "choice=  Select [1-9/0/E]: "
if /i "%choice%"=="1" goto ads_menu
if /i "%choice%"=="2" goto frp_menu
if /i "%choice%"=="3" goto bloat_menu
if /i "%choice%"=="4" goto utils_menu
if /i "%choice%"=="5" goto privacy_menu
if /i "%choice%"=="6" goto app_menu
if /i "%choice%"=="7" goto quick_menu
if /i "%choice%"=="8" goto dev_menu
if /i "%choice%"=="9" goto net_menu
if /i "%choice%"=="0" goto settings_menu
if /i "%choice%"=="E" goto end
goto main_menu

:: ====================================================================
::                     SECTION 1: AD REMOVAL
:: ====================================================================
:ads_menu
cls
echo.
echo   =======================================================================
echo   ^|                     AD REMOVAL TOOLKIT                              ^|
echo   =======================================================================
echo.
echo   +----------------------------+  +----------------------------+
echo   ^|      [1] HOSTS FILE BLOCK  ^|  ^|      [2] DISABLE AD SVCS  ^|
echo   ^|   130+ domains, root       ^|  ^|   Google + 40 SDKs         ^|
echo   +----------------------------+  +----------------------------+
echo   +----------------------------+  +----------------------------+
echo   ^|      [3] NUCLEAR OPTION    ^|  ^|      [4] DNS-BASED BLOCK  ^|
echo   ^|   All methods combined     ^|  ^|   7 DNS providers         ^|
echo   +----------------------------+  +----------------------------+
echo   +----------------------------+  +----------------------------+
echo   ^|      [5] STOP TRACKING     ^|  ^|      [6] CUSTOM HOSTS     ^|
echo   ^|   Reset ID, revoke perms   ^|  ^|   View/edit/add/remove    ^|
echo   +----------------------------+  +----------------------------+
echo   +----------------------------+  +----------------------------+
echo   ^|      [7] FULL ADS CLEAN    ^|  ^|      [8] BANNER REMOVAL   ^|
echo   ^|   Everything combined      ^|  ^|   Overlay, popup, inter.  ^|
echo   +----------------------------+  +----------------------------+
echo   +----------------------------+  +----------------------------+
echo   ^|      [9] AD PERMISSIONS    ^|  ^|      [0] BACK              ^|
echo   ^|   Camera, loc, phone, SMS  ^|  ^|   Return to dashboard      ^|
echo   +----------------------------+  +----------------------------+
echo.
set /p "ads_choice=  Select [1-9/0]: "
if "%ads_choice%"=="1" goto ads_hosts
if "%ads_choice%"=="2" goto ads_services
if "%ads_choice%"=="3" goto ads_nuclear
if "%ads_choice%"=="4" goto ads_dns
if "%ads_choice%"=="5" goto ads_tracking
if "%ads_choice%"=="6" goto ads_custom
if "%ads_choice%"=="7" goto ads_full
if "%ads_choice%"=="8" goto ads_banner
if "%ads_choice%"=="9" goto ads_perms
if "%ads_choice%"=="0" goto main_menu
goto ads_menu

:ads_hosts
cls
echo.
echo   =======================================================================
echo   ^|               HOSTS FILE AD BLOCKING                               ^|
echo   =======================================================================
echo.
if "%ROOTED%"=="0" (
    echo   [ERROR] Root required! Use option [4] DNS-based instead.
    pause
    goto ads_menu
)
echo   [*] Backing up hosts...
adb shell su -c "cp /system/etc/hosts /system/etc/hosts.bak.bnt" 2>nul
adb shell su -c "mount -o rw,remount /system" 2>nul
echo   [*] Writing 130+ ad domains...
(
echo 127.0.0.1 localhost
echo :: === GOOGLE ADS ===
echo 127.0.0.1 pagead2.googlesyndication.com
echo 127.0.0.1 adservice.google.com
echo 127.0.0.1 googleads.g.doubleclick.net
echo 127.0.0.1 www.googleadservices.com
echo 127.0.0.1 ad.doubleclick.net
echo 127.0.0.1 doubleclick.net
echo 127.0.0.1 fls.doubleclick.net
echo 127.0.0.1 stats.g.doubleclick.net
echo 127.0.0.1 googlesyndication.com
echo 127.0.0.1 www.googlesyndication.com
echo :: === GOOGLE ANALYTICS ===
echo 127.0.0.1 analytics.google.com
echo 127.0.0.1 www.google-analytics.com
echo 127.0.0.1 google-analytics.com
echo 127.0.0.1 googletagmanager.com
echo 127.0.0.1 www.googletagmanager.com
echo 127.0.0.1 app-measurement.com
echo 127.0.0.1 www.app-measurement.com
echo 127.0.0.1 chartbeat.net
echo 127.0.0.1 www.chartbeat.net
echo 127.0.0.1 scorecardresearch.com
echo 127.0.0.1 www.scorecardresearch.com
echo :: === FACEBOOK/META ===
echo 127.0.0.1 facebook.com
echo 127.0.0.1 www.facebook.com
echo 127.0.0.1 graph.facebook.com
echo 127.0.0.1 pixel.facebook.com
echo 127.0.0.1 an.facebook.com
echo 127.0.0.1 b-graph.facebook.com
echo 127.0.0.1 b-api.facebook.com
echo 127.0.0.1 tr.facebook.com
echo :: === AD NETWORKS ===
echo 127.0.0.1 adobedtm.com
echo 127.0.0.1 amazon-adsystem.com
echo 127.0.0.1 ad.turn.com
echo 127.0.0.1 ads.mopub.com
echo 127.0.0.1 ads.yahoo.com
echo 127.0.0.1 moatads.com
echo 127.0.0.1 mopub.com
echo 127.0.0.1 openx.net
echo 127.0.0.1 www.openx.net
echo 127.0.0.1 outbrain.com
echo 127.0.0.1 www.outbrain.com
echo 127.0.0.1 revcontent.com
echo 127.0.0.1 taboola.com
echo 127.0.0.1 www.taboola.com
echo 127.0.0.1 media.net
echo 127.0.0.1 www.media.net
echo 127.0.0.1 adnxs.com
echo 127.0.0.1 www.adnxs.com
echo 127.0.0.1 casalemedia.com
echo 127.0.0.1 demdex.net
echo 127.0.0.1 pubmatic.com
echo 127.0.0.1 rubiconproject.com
echo 127.0.0.1 quantserve.com
echo :: === AD SDKs ===
echo 127.0.0.1 adcolony.com
echo 127.0.0.1 airpush.com
echo 127.0.0.1 tapjoy.com
echo 127.0.0.1 vungle.com
echo 127.0.0.1 zedo.com
echo 127.0.0.1 inmobi.com
echo 127.0.0.1 unity3d.com
echo 127.0.0.1 unityads.unity3d.com
echo 127.0.0.1 smaato.net
echo 127.0.0.1 fyber.com
echo 127.0.0.1 yieldmo.com
echo 127.0.0.1 nativo.com
echo 127.0.0.1 ads-twitter.com
echo 127.0.0.1 ads.snapchat.com
echo :: === TRACKING SDKs ===
echo 127.0.0.1 adjust.com
echo 127.0.0.1 app.adjust.com
echo 127.0.0.1 appsflyer.com
echo 127.0.0.1 kochava.com
echo 127.0.0.1 braze.com
echo 127.0.0.1 appboy.com
echo 127.0.0.1 segment.com
echo 127.0.0.1 amplitude.com
echo 127.0.0.1 mixpanel.com
echo 127.0.0.1 hotjar.com
echo 127.0.0.1 fullstory.com
echo 127.0.0.1 localytics.com
echo 127.0.0.1 urbanairship.com
echo 127.0.0.1 leanplum.com
echo 127.0.0.1 onesignal.com
echo 127.0.0.1 pushwoosh.com
echo 127.0.0.1 flurry.com
echo 127.0.0.1 ironsrc.com
echo 127.0.0.1 startapp.com
echo 127.0.0.1 chartboost.com
echo 127.0.0.1 leadbolt.com
echo 127.0.0.1 mparticle.com
echo 127.0.0.1 branch.io
echo 127.0.0.1 singular.net
echo 127.0.0.1 tenjin.com
echo :: === RETARGETING ===
echo 127.0.0.1 criteo.com
echo 127.0.0.1 www.criteo.com
echo 127.0.0.1 criteo.net
echo 127.0.0.1 mathtag.com
echo 127.0.0.1 bluekai.com
echo 127.0.0.1 exelator.com
echo 127.0.0.1 eyeota.net
echo :: === FRAUD/MALVERTISING ===
echo 127.0.0.1 popads.net
echo 127.0.0.1 propellerads.com
echo 127.0.0.1 exoclick.com
echo 127.0.0.1 juicyads.com
echo 127.0.0.1 trafficjunky.com
echo 127.0.0.1 adnium.com
echo 127.0.0.1 clickadu.com
echo 127.0.0.1 hilltopads.com
echo 127.0.0.1 galaksion.com
echo :: === CRYPTOMINERS ===
echo 127.0.0.1 coinhive.com
echo 127.0.0.1 coin-hive.com
echo 127.0.0.1 jsecoin.com
echo 127.0.0.1 crypto-loot.com
echo 127.0.0.1 minr.pw
) > "%TEMP%\bnt\hosts_adblock"
adb push "%TEMP%\bnt\hosts_adblock" /sdcard/hosts_adblock >nul 2>&1
adb shell su -c "cp /sdcard/hosts_adblock /system/etc/hosts" 2>nul
adb shell su -c "chmod 644 /system/etc/hosts" 2>nul
adb shell su -c "mount -o ro,remount /system" 2>nul
adb shell rm /sdcard/hosts_adblock 2>nul
del "%TEMP%\bnt\hosts_adblock" 2>nul
echo.
echo   [OK] 130+ ad domains blocked via hosts file.
echo [%date% %time%] Hosts file applied >> "%LOG%"
pause
goto ads_menu

:ads_services
cls
echo.
echo   =======================================================================
echo   ^|               DISABLE AD SERVICES                                  ^|
echo   =======================================================================
echo.
echo   [1/9] Google ad components...
for %%G in (com.google.android.gms.ads com.google.android.gms.ads.admanager com.google.android.gms.analytics com.google.android.apps.ads.services) do adb shell pm disable-user --user 0 %%G 2>nul
echo   [DONE]
echo   [2/9] Ad SDKs (40+)...
for %%P in (com.applovin com.inmobi com.mopub com.facebook.ads com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.flurry com.chartbeat com.revmob com.nativex com.hyprmx com.verve com.millennialmedia com.chartboost com.leadbolt com.startapp com.airpush) do adb shell pm disable-user --user 0 %%P 2>nul
echo   [DONE]
echo   [3/9] Ad system services...
for %%S in (com.google.android.gms.games com.google.android.googlequicksearchbox) do adb shell pm disable-user --user 0 %%S 2>nul
echo   [DONE]
echo   [4/9] Limit ad tracking...
adb shell settings put secure limit_ad_tracking 1 2>nul
echo   [DONE]
echo   [5/9] Disable personalized ads...
adb shell settings put secure interest_based_ad 0 2>nul
adb shell settings put global ad_id_opt_out 1 2>nul
echo   [DONE]
echo   [6/9] Clear ad data...
for %%C in (com.google.android.gms com.google.android.gms.ads com.google.android.gms.analytics com.facebook.katana) do adb shell pm clear %%C 2>nul
echo   [DONE]
echo   [7/9] Force-stop ad processes...
for %%F in (com.google.android.gms.ads com.facebook.ads com.applovin com.mopub com.unity3d.services) do adb shell am force-stop %%F 2>nul
echo   [DONE]
echo   [8/9] Revoke ad permissions...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.GET_ACCOUNTS 2>nul
)
echo   [DONE]
echo   [9/9] Flush DNS...
adb shell cmd connectivity flush-dns 2>nul
echo   [DONE]
echo.
echo   [OK] All 9 steps completed.
echo [%date% %time%] Ad services disabled >> "%LOG%"
pause
goto ads_menu

:ads_nuclear
cls
echo.
echo   =======================================================================
echo   ^|               NUCLEAR AD REMOVAL                                   ^|
echo   =======================================================================
echo.
echo   WARNING: ALL ad-blocking methods at once!
set /p "confirm=  Continue? (Y/N): "
if /i not "%confirm%"=="Y" goto ads_menu
echo.
echo   [1/4] Disabling ad packages...
for %%P in (com.google.android.gms.ads com.google.android.gms.ads.admanager com.google.android.gms.analytics com.google.android.apps.ads.services com.applovin com.inmobi com.mopub com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.millennialmedia com.flurry com.facebook.ads com.startapp com.chartboost com.leadbolt) do adb shell pm disable-user --user 0 %%P 2>nul
echo   [DONE]
echo   [2/4] Privacy + DNS...
adb shell settings put secure limit_ad_tracking 1 2>nul
adb shell settings put secure interest_based_ad 0 2>nul
adb shell settings put global ad_id_opt_out 1 2>nul
adb shell settings put global private_dns_mode hostname 2>nul
adb shell settings put global private_dns_specifier dns.adguard.com 2>nul
echo   [DONE]
echo   [3/4] Revoking tracking permissions...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
)
echo   [DONE]
echo   [4/4] Clearing data + flush DNS...
for %%C in (com.google.android.gms com.google.android.gms.ads com.facebook.katana) do adb shell pm clear %%C 2>nul
adb shell cmd connectivity flush-dns 2>nul
echo   [DONE]
echo.
echo   [OK] NUCLEAR AD REMOVAL COMPLETE.
echo [%date% %time%] Nuclear ad removal >> "%LOG%"
pause
goto ads_menu

:ads_dns
cls
echo.
echo   =======================================================================
echo   ^|               DNS-BASED AD BLOCKING (No Root)                      ^|
echo   =======================================================================
echo.
echo   [1] AdGuard DNS         - dns.adguard.com
echo   [2] AdGuard Family      - family.adguard-dns.com
echo   [3] NextDNS             - dns.nextdns.io
echo   [4] NextDNS Ads         - ads-dns.nextdns.io
echo   [5] OpenDNS FamilyShield- dofamilyshield.opendns.com
echo   [6] Cloudflare Security - security.cloudflare-dns.com
echo   [7] CleanBrowsing       - security.cleanbrowsing.org
echo   [8] Custom DNS
echo   [9] Remove DNS blocking
echo   [0] Back
echo.
set /p "dns_choice=  Select [1-9/0]: "
if "%dns_choice%"=="1" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier dns.adguard.com 2>nul & echo   [OK] AdGuard DNS set)
if "%dns_choice%"=="2" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier family.adguard-dns.com 2>nul & echo   [OK] AdGuard Family set)
if "%dns_choice%"=="3" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier dns.nextdns.io 2>nul & echo   [OK] NextDNS set)
if "%dns_choice%"=="4" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier ads-dns.nextdns.io 2>nul & echo   [OK] NextDNS Ads set)
if "%dns_choice%"=="5" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier dofamilyshield.opendns.com 2>nul & echo   [OK] OpenDNS FamilyShield set)
if "%dns_choice%"=="6" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier security.cloudflare-dns.com 2>nul & echo   [OK] Cloudflare Security set)
if "%dns_choice%"=="7" (adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier security.cleanbrowsing.org 2>nul & echo   [OK] CleanBrowsing set)
if "%dns_choice%"=="8" (set /p "custom_dns=  DNS hostname: " & adb shell settings put global private_dns_mode hostname 2>nul & adb shell settings put global private_dns_specifier "!custom_dns!" 2>nul & echo   [OK] Custom: !custom_dns!)
if "%dns_choice%"=="9" (adb shell settings put global private_dns_mode off 2>nul & adb shell settings delete global private_dns_specifier 2>nul & echo   [OK] DNS blocking removed)
if "%dns_choice%"=="0" goto ads_menu
echo [%date% %time%] DNS configured >> "%LOG%"
pause
goto ads_dns

:ads_tracking
cls
echo.
echo   =======================================================================
echo   ^|               STOP TRACKING & RESET AD ID                          ^|
echo   =======================================================================
echo.
echo   [1/6] Reset Ad ID...
adb shell settings put secure advertising_id "" 2>nul
echo   [2/6] Limit ad tracking...
adb shell settings put secure limit_ad_tracking 1 2>nul
echo   [3/6] Disable tracking settings...
adb shell settings put global ad_id_opt_out 1 2>nul
adb shell settings put secure interest_based_ad 0 2>nul
adb shell settings put secure interest_based_ads 0 2>nul
echo   [4/6] App-level tracking...
for %%P in (com.google.android.gms com.google.android.gms.ads com.google.android.gms.analytics) do (
    adb shell appops set %%P TRACK_AUDIENCE deny 2>nul
    adb shell appops set %%P READ_PHONE_STATE deny 2>nul
    adb shell appops set %%P ACCESS_FINE_LOCATION deny 2>nul
    adb shell appops set %%P ACCESS_COARSE_LOCATION deny 2>nul
    adb shell appops set %%P GET_ACCOUNTS deny 2>nul
)
echo   [5/6] Revoke from user apps...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.READ_CONTACTS 2>nul
    adb shell pm revoke %%P android.permission.READ_SMS 2>nul
)
echo   [6/6] Disable usage stats...
adb shell appops set com.google.android.gms USAGE_STATS deny 2>nul
adb shell settings put secure usage_metrics_reporting_enabled 0 2>nul
echo.
echo   [OK] Tracking fully disabled.
echo [%date% %time%] Tracking disabled >> "%LOG%"
pause
goto ads_menu

:ads_custom
cls
echo.
echo   =======================================================================
echo   ^|               CUSTOM HOSTS EDITOR                                  ^|
echo   =======================================================================
echo.
if "%ROOTED%"=="0" (echo   [ERROR] Root required. & pause & goto ads_menu)
echo   [1] View   [2] Add domain   [3] Remove   [4] Restore   [5] Reset   [0] Back
echo.
set /p "hc=  Select [1-5/0]: "
if "%hc%"=="1" (echo. & adb shell su -c "cat /system/etc/hosts" 2>nul & echo.)
if "%hc%"=="2" (set /p "dom=  Domain to block: " & adb shell su -c "echo '127.0.0.1 !dom!' >> /system/etc/hosts" 2>nul & echo   [OK] !dom! blocked)
if "%hc%"=="3" (set /p "rdom=  Domain to unblock: " & adb shell su -c "sed -i '/!rdom!/d' /system/etc/hosts" 2>nul & echo   [OK] !rdom! removed)
if "%hc%"=="4" (adb shell su -c "cp /system/etc/hosts.bak.bnt /system/etc/hosts" 2>nul & echo   [OK] Restored)
if "%hc%"=="5" (adb shell su -c "echo '127.0.0.1 localhost' > /system/etc/hosts" 2>nul & echo   [OK] Reset)
if "%hc%"=="0" goto ads_menu
pause
goto ads_custom

:ads_full
cls
echo.
echo   =======================================================================
echo   ^|               COMPLETE AD CLEAN                                    ^|
echo   =======================================================================
echo.
set /p "confirm=  Run ALL ad methods? (Y/N): "
if /i not "%confirm%"=="Y" goto ads_menu
echo.
echo   [1/4] Disabling ad packages...
for %%P in (com.google.android.gms.ads com.google.android.gms.ads.admanager com.google.android.gms.analytics com.google.android.apps.ads.services com.applovin com.inmobi com.mopub com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.millennialmedia com.flurry com.facebook.ads com.startapp com.chartboost) do adb shell pm disable-user --user 0 %%P 2>nul
echo   [DONE]
echo   [2/4] DNS + privacy...
adb shell settings put secure limit_ad_tracking 1 2>nul
adb shell settings put secure interest_based_ad 0 2>nul
adb shell settings put global ad_id_opt_out 1 2>nul
adb shell settings put global private_dns_mode hostname 2>nul
adb shell settings put global private_dns_specifier dns.adguard.com 2>nul
echo   [DONE]
echo   [3/4] Revoking permissions...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.GET_ACCOUNTS 2>nul
)
echo   [DONE]
echo   [4/4] Clearing data...
for %%C in (com.google.android.gms com.google.android.gms.ads com.facebook.katana) do adb shell pm clear %%C 2>nul
adb shell cmd connectivity flush-dns 2>nul
echo   [DONE]
echo.
echo   [OK] COMPLETE AD CLEAN FINISHED.
echo [%date% %time%] Full ad clean >> "%LOG%"
pause
goto ads_menu

:ads_banner
cls
echo.
echo   =======================================================================
echo   ^|               ADB BANNER REMOVAL                                  ^|
echo   =======================================================================
echo.
echo   [1] Disable overlay   [2] Disable popups   [3] Block interstitials
echo   [4] System-wide ad block   [0] Back
echo.
set /p "bc=  Select [1-4/0]: "
if "%bc%"=="1" (adb shell settings put global overlay_settings_enabled 0 2>nul & echo   [OK] Overlay disabled)
if "%bc%"=="2" (adb shell settings put secure popup_settings_value 0 2>nul & echo   [OK] Popups disabled)
if "%bc%"=="3" (adb shell settings put global interceptor_ad_interstitial 0 2>nul & echo   [OK] Interstitials blocked)
if "%bc%"=="4" (adb shell settings put global ad_blocker_enabled 1 2>nul & adb shell settings put global system_ad_blocker 1 2>nul & echo   [OK] System-wide block enabled)
if "%bc%"=="0" goto ads_menu
pause
goto ads_banner

:ads_perms
cls
echo.
echo   =======================================================================
echo   ^|               REVOKE AD PERMISSIONS                                ^|
echo   =======================================================================
echo.
echo   [1/4] Revoking camera...
for %%P in (com.google.android.gms com.facebook.katana com.applovin com.inmobi) do adb shell pm revoke %%P android.permission.CAMERA 2>nul
echo   [2/4] Revoking location...
for %%P in (com.google.android.gms com.facebook.katana com.applovin) do (
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_BACKGROUND_LOCATION 2>nul
)
echo   [3/4] Revoking phone/SMS...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.READ_SMS 2>nul
    adb shell pm revoke %%P android.permission.READ_CALL_LOG 2>nul
    adb shell pm revoke %%P android.permission.READ_CONTACTS 2>nul
)
echo   [4/4] Revoking storage...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_EXTERNAL_STORAGE 2>nul
    adb shell pm revoke %%P android.permission.WRITE_EXTERNAL_STORAGE 2>nul
)
echo.
echo   [OK] All ad permissions revoked.
pause
goto ads_menu

:: ====================================================================
::                     SECTION 2: FRP BYPASS
:: ====================================================================
:frp_menu
cls
echo.
echo   =======================================================================
echo   ^|                     FRP BYPASS TOOLKIT                              ^|
echo   =======================================================================
echo.
echo   WARNING: Only use on devices you legally own!
echo.
echo   [1]  Bypass Setup Wizard      [7]  Full FRP Bypass
echo   [2]  Open Settings             [8]  ADB Shell Access
echo   [3]  Remove Google Account     [9]  OEM Unlock
echo   [4]  Clear GAM Data            [10] Wipe Data/Reset
echo   [5]  Disable FRP Lock          [11] Accessibility Method
echo   [6]  Launch Browser            [12] Disable Find My Device
echo                                  [13] Fastboot FRP Bypass
echo   [0]  Back to Dashboard
echo.
set /p "frp_choice=  Select [1-13/0]: "
if "%frp_choice%"=="1" goto frp_setup
if "%frp_choice%"=="2" goto frp_settings
if "%frp_choice%"=="3" goto frp_account
if "%frp_choice%"=="4" goto frp_clear
if "%frp_choice%"=="5" goto frp_disable
if "%frp_choice%"=="6" goto frp_browser
if "%frp_choice%"=="7" goto frp_all
if "%frp_choice%"=="8" goto frp_shell
if "%frp_choice%"=="9" goto frp_oem
if "%frp_choice%"=="10" goto frp_wipe
if "%frp_choice%"=="11" goto frp_access
if "%frp_choice%"=="12" goto frp_fmd
if "%frp_choice%"=="13" goto frp_fastboot
if "%frp_choice%"=="0" goto main_menu
goto frp_menu

:frp_setup
cls
echo.
echo   === BYPASS SETUP WIZARD ===
echo   [1/6] Setting provisioned...
adb shell settings put global device_provisioned 1 2>nul
echo   [2/6] Marking setup complete...
adb shell settings put secure user_setup_complete 1 2>nul
echo   [3/6] Disabling wizard packages...
for %%S in (com.google.android.setupwizard com.sec.android.app.SecSetupWizard com.miui.miservice com.huawei.android.hwfrozen com.oppo.setupwizard com.heytap.setupwizard com.vivo.setupwizard com.zte.setupwizard com.motorola.setupwizard) do adb shell pm disable-user --user 0 %%S 2>nul
echo   [4/6] Killing processes...
adb shell am force-stop com.google.android.setupwizard 2>nul
adb shell am force-stop com.sec.android.app.SecSetupWizard 2>nul
echo   [5/6] Clearing data...
adb shell pm clear com.google.android.setupwizard 2>nul
echo   [6/6] Going home...
adb shell am start -a android.intent.action.MAIN -c android.intent.category.HOME 2>nul
echo.
echo   [OK] Setup Wizard bypassed!
echo [%date% %time%] Setup Wizard bypassed >> "%LOG%"
pause
goto frp_menu

:frp_settings
cls
echo.
echo   === OPEN SETTINGS ===
echo   [1] Main  [2] Developer  [3] Accessibility  [4] Security
echo   [5] Accounts  [6] Apps  [7] WiFi  [8] About  [9] All  [0] Back
echo.
set /p "sc=  Select: "
if "%sc%"=="1" adb shell am start com.android.settings/com.android.settings.Settings 2>nul
if "%sc%"=="2" adb shell am start com.android.settings/com.android.settings.DevelopmentSettings 2>nul
if "%sc%"=="3" adb shell am start -a android.settings.ACCESSIBILITY_SETTINGS 2>nul
if "%sc%"=="4" adb shell am start -a android.settings.SECURITY_SETTINGS 2>nul
if "%sc%"=="5" adb shell am start -a android.settings.SYNC_SETTINGS 2>nul
if "%sc%"=="6" adb shell am start -a android.settings.APPLICATION_SETTINGS 2>nul
if "%sc%"=="7" adb shell am start -a android.settings.WIFI_SETTINGS 2>nul
if "%sc%"=="8" adb shell am start -a android.settings.DEVICE_INFO_SETTINGS 2>nul
if "%sc%"=="9" (adb shell am start com.android.settings/com.android.settings.Settings 2>nul & adb shell am start -a android.settings.ACCESSIBILITY_SETTINGS 2>nul)
if "%sc%"=="0" goto frp_menu
echo   [OK] Settings opened.
pause
goto frp_settings

:frp_account
cls
echo.
echo   === REMOVE GOOGLE ACCOUNT ===
echo   [1/5] Clear login data...
adb shell pm clear com.google.android.gsf.login 2>nul
adb shell pm clear com.google.android.gsf 2>nul
echo   [2/5] Clear GMS auth...
adb shell pm clear com.google.android.gms 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.auth.authzen 2>nul
echo   [3/5] Clear trust/FIDO...
adb shell pm clear com.google.android.gms.trust 2>nul
adb shell pm clear com.google.android.gms.fido 2>nul
echo   [4/5] Disable/re-enable GMS...
adb shell pm disable-user --user 0 com.google.android.gms 2>nul
adb shell am force-stop com.google.android.gms 2>nul
timeout /t 2 >nul
adb shell pm enable com.google.android.gms 2>nul
echo   [5/5] Clear sync data...
adb shell settings delete secure sync1 2>nul
adb shell settings delete secure sync2 2>nul
echo.
echo   [OK] Google account removed.
echo [%date% %time%] Google account removed >> "%LOG%"
pause
goto frp_menu

:frp_clear
cls
echo.
echo   === CLEAR GOOGLE ACCOUNT MANAGER DATA ===
echo   [1/7] GSF...
adb shell pm clear com.google.android.gsf 2>nul
adb shell pm clear com.google.android.gsf.login 2>nul
echo   [2/7] GMS auth variants...
adb shell pm clear com.google.android.gms 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.auth.authzen 2>nul
adb shell pm clear com.google.android.gms.auth.cryptauth 2>nul
echo   [3/7] Trust/FIDO/payment...
adb shell pm clear com.google.android.gms.trust 2>nul
adb shell pm clear com.google.android.gms.fido 2>nul
adb shell pm clear com.google.android.gms.tapandpay 2>nul
adb shell pm clear com.google.android.gms.wallet 2>nul
echo   [4/7] Account databases...
adb shell rm -rf /data/system/users/*/accounts.db 2>nul
adb shell rm -rf /data/system/users/*/accounts_de.db 2>nul
adb shell rm -rf /data/system/users/*/accounts_ce.db 2>nul
echo   [5/7] GMS databases...
adb shell rm -rf /data/data/com.google.android.gms/databases/* 2>nul
echo   [6/7] Cached settings...
adb shell rm -rf /data/system/users/*/settings_secure.xml 2>nul
echo   [7/7] Setup data...
adb shell rm -rf /data/data/com.google.android.gsf.login/databases/* 2>nul
echo.
echo   [OK] GAM data fully cleared.
echo [%date% %time%] GAM cleared >> "%LOG%"
pause
goto frp_menu

:frp_disable
cls
echo.
echo   === DISABLE FRP LOCK ===
echo   [1/5] FRP flag...
adb shell settings put secure frp_mode_disabled 1 2>nul
echo   [2/5] Provisioned...
adb shell settings put global device_provisioned 1 2>nul
echo   [3/5] Setup complete...
adb shell settings put secure user_setup_complete 1 2>nul
echo   [4/5] Disable wizard...
adb shell pm disable-user --user 0 com.google.android.setupwizard 2>nul
adb shell pm disable-user --user 0 com.google.android.gsf.login 2>nul
echo   [5/5] Clear FRP data...
adb shell pm clear com.google.android.gsf.login 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.trust 2>nul
echo.
echo   [OK] FRP lock disabled.
echo [%date% %time%] FRP disabled >> "%LOG%"
pause
goto frp_menu

:frp_browser
cls
echo.
echo   === LAUNCH BROWSER ===
echo   [1] Recovery page   [2] Chrome   [3] Samsung Internet
echo   [4] Firefox   [5] YouTube   [6] FRP Tools   [7] Custom URL   [0] Back
echo.
set /p "brwc=  Select: "
if "%brwc%"=="1" adb shell am start -a android.intent.action.VIEW -d "https://accounts.google.com/signin/recovery" 2>nul
if "%brwc%"=="2" adb shell am start -n com.android.chrome/com.google.android.apps.chrome.Main 2>nul
if "%brwc%"=="3" adb shell am start -n com.sec.android.app.sbrowser/com.sec.android.app.sbrowser.SBrowserMainActivity 2>nul
if "%brwc%"=="4" adb shell am start -n org.mozilla.firefox/org.mozilla.firefox.App 2>nul
if "%brwc%"=="5" adb shell am start -a android.intent.action.VIEW -d "https://youtube.com" 2>nul
if "%brwc%"=="6" (adb shell am start -a android.intent.action.VIEW -d "https://frpbypass.io" 2>nul)
if "%brwc%"=="7" (set /p "curl=  URL: " & adb shell am start -a android.intent.action.VIEW -d "!curl!" 2>nul)
if "%brwc%"=="0" goto frp_menu
echo   [OK] Browser opened.
pause
goto frp_browser

:frp_all
cls
echo.
echo   === FULL FRP BYPASS (10 STEPS) ===
set /p "confirm=  Continue? (Y/N): "
if /i not "%confirm%"=="Y" goto frp_menu
echo.
echo   [1/10] FRP flag...
adb shell settings put secure frp_mode_disabled 1 2>nul
echo   [2/10] Provisioned...
adb shell settings put global device_provisioned 1 2>nul
echo   [3/10] Setup complete...
adb shell settings put secure user_setup_complete 1 2>nul
echo   [4/10] Clear Google data...
for %%C in (com.google.android.gsf.login com.google.android.gsf com.google.android.gms com.google.android.gms.auth com.google.android.gms.auth.authzen com.google.android.gms.auth.cryptauth com.google.android.gms.trust com.google.android.gms.fido) do adb shell pm clear %%C 2>nul
echo   [5/10] Disable wizards...
for %%S in (com.google.android.setupwizard com.sec.android.app.SecSetupWizard com.miui.miservice com.huawei.android.hwfrozen) do adb shell pm disable-user --user 0 %%S 2>nul
echo   [6/10] Force-stop...
adb shell am force-stop com.google.android.setupwizard 2>nul
echo   [7/10] Clear FRP DBs...
adb shell rm -rf /data/system/users/*/accounts.db 2>nul
adb shell rm -rf /data/system/users/*/accounts_de.db 2>nul
echo   [8/10] Disable Find My Device...
adb shell pm disable-user --user 0 com.google.android.gms.trust 2>nul
echo   [9/10] Go home...
adb shell am start -a android.intent.action.MAIN -c android.intent.category.HOME 2>nul
echo   [10/10] Open Settings...
adb shell am start com.android.settings/com.android.settings.Settings 2>nul
echo.
echo   [OK] FULL FRP BYPASS COMPLETED!
echo [%date% %time%] Full FRP bypass >> "%LOG%"
pause
goto frp_menu

:frp_shell
cls
echo.
echo   === ADB SHELL ACCESS ===
echo   Useful: settings put global device_provisioned 1
echo   Type "exit" to return.
echo   -------------------------------------------------------
adb shell
echo.
pause
goto frp_menu

:frp_oem
cls
echo.
echo   === ENABLE OEM UNLOCKING ===
echo   [1/4] Developer options...
for /L %%I in (1,1,7) do adb shell settings put global development_settings_enabled 1 2>nul
echo   [2/4] USB debugging...
adb shell settings put global adb_enabled 1 2>nul
echo   [3/4] OEM unlock...
adb shell settings put global oem_unlock_enabled 1 2>nul
echo   [4/4] Bootloader unlock...
adb shell oem unlock 2>nul
echo.
echo   [OK] OEM unlocking enabled.
pause
goto frp_menu

:frp_wipe
cls
echo.
echo   === WIPE DATA / FACTORY RESET ===
echo   WARNING: ALL DATA WILL BE ERASED!
set /p "confirm=  Type YES to confirm: "
if not "%confirm%"=="YES" (echo   [CANCELLED] & pause & goto frp_menu)
echo   [1/3] Wiping data...
adb shell recovery --wipe_data 2>nul
echo   [2/3] Wiping cache...
adb shell recovery --wipe_cache 2>nul
echo   [3/3] Rebooting...
adb reboot recovery 2>nul
echo.
echo   [OK] Factory reset initiated.
echo [%date% %time%] Factory reset >> "%LOG%"
pause
goto frp_menu

:frp_access
cls
echo.
echo   === ACCESSIBILITY/TALKBACK METHOD ===
echo   [1/4] Enabling TalkBack...
adb shell settings put secure enabled_accessibility_services com.google.android.marvin.talkback/com.google.android.marvin.talkback.TalkBackService 2>nul
adb shell settings put secure accessibility_enabled 1 2>nul
echo   [2/4] Open accessibility...
adb shell am start -a android.settings.ACCESSIBILITY_SETTINGS 2>nul
echo   [3/4] Open Google app...
adb shell am start -n com.google.android.googlequicksearchbox/com.google.android.launcher.GEL 2>nul
echo   [4/4] Drawing L gesture...
adb shell input swipe 100 500 100 100 300 2>nul
echo.
echo   [OK] TalkBack method initiated.
pause
goto frp_menu

:frp_fmd
cls
echo.
echo   === DISABLE FIND MY DEVICE ===
echo   [1/3] Disabling FMD...
adb shell pm disable-user --user 0 com.google.android.gms.trust 2>nul
adb shell pm disable-user --user 0 com.google.android.gms 2>nul
echo   [2/3] Disabling location...
adb shell settings put secure location_mode 0 2>nul
echo   [3/3] Clear FMD data...
adb shell pm clear com.google.android.gms.trust 2>nul
adb shell pm clear com.google.android.gms.auth.trustagent 2>nul
echo.
echo   [OK] Find My Device disabled.
pause
goto frp_menu

:frp_fastboot
cls
echo.
echo   =======================================================================
echo   ^|               FASTBOOT FRP BYPASS                                  ^|
echo   =======================================================================
echo.
echo   Device must be in fastboot mode (bootloader).
echo   Use option [1] to reboot to bootloader first.
echo.
echo   [1]  Reboot to Bootloader (Fastboot)
echo   [2]  Erase FRP Partition
echo   [3]  Erase Persist Partition
echo   [4]  Erase FRP + Persist + Cache + Userdata
echo   [5]  OEM Unlock (fastboot)
echo   [6]  Unlock Bootloader (full)
echo   [7]  Reboot to Recovery
echo   [8]  Reboot System
echo   [9]  Fastboot Device Info
echo   [A]  Custom Fastboot Command
echo   [0]  Back
echo.
set /p "fb_choice=  Select: "

if "%fb_choice%"=="1" (
    echo   [*] Rebooting to bootloader...
    adb reboot bootloader 2>nul
    echo   [OK] Device should be in fastboot mode now.
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="2" (
    set /p "confirm=  Erase FRP partition? (Y/N): "
    if /i "!confirm!"=="Y" (
        echo   [*] Erasing FRP partition...
        fastboot erase frp 2>nul
        echo   [*] Rebooting...
        fastboot reboot 2>nul
        echo   [OK] FRP partition erased.
    )
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="3" (
    set /p "confirm=  Erase persist partition? (Y/N): "
    if /i "!confirm!"=="Y" (
        echo   [*] Erasing persist partition...
        fastboot erase persist 2>nul
        echo   [*] Rebooting...
        fastboot reboot 2>nul
        echo   [OK] Persist partition erased.
    )
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="4" (
    set /p "confirm=  Erase FRP+persist+cache+userdata? (Y/N): "
    if /i "!confirm!"=="Y" (
        echo   [1/5] Erasing FRP...
        fastboot erase frp 2>nul
        echo   [2/5] Erasing persist...
        fastboot erase persist 2>nul
        echo   [3/5] Erasing cache...
        fastboot erase cache 2>nul
        echo   [4/5] Erasing userdata...
        fastboot erase userdata 2>nul
        echo   [5/5] Rebooting...
        fastboot reboot 2>nul
        echo   [OK] All partitions erased.
    )
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="5" (
    set /p "confirm=  Send OEM unlock? (Y/N): "
    if /i "!confirm!"=="Y" (
        echo   [1/2] fastboot oem unlock...
        fastboot oem unlock 2>nul
        echo   [2/2] fastboot flashing unlock...
        fastboot flashing unlock 2>nul
        echo   [OK] OEM unlock commands sent.
    )
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="6" (
    set /p "confirm=  Unlock bootloader? WIPE ALL DATA! (Y/N): "
    if /i "!confirm!"=="Y" (
        echo   [1/3] fastboot oem unlock...
        fastboot oem unlock 2>nul
        echo   [2/3] fastboot flashing unlock...
        fastboot flashing unlock 2>nul
        echo   [3/3] fastboot flashing unlock_critical...
        fastboot flashing unlock_critical 2>nul
        echo   [OK] Bootloader unlock commands sent.
    )
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="7" (
    echo   [*] Rebooting to recovery...
    fastboot reboot recovery 2>nul
    echo   [OK] Rebooting to recovery.
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="8" (
    echo   [*] Rebooting to system...
    fastboot reboot 2>nul
    echo   [OK] Rebooting.
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="9" (
    echo.
    echo   === FASTBOOT DEVICE INFO ===
    echo   Product: 
    fastboot getvar product 2>nul
    echo   Serialno: 
    fastboot getvar serialno 2>nul
    echo   Unlocked: 
    fastboot getvar unlocked 2>nul
    echo   Secure: 
    fastboot getvar secure 2>nul
    echo   Variant: 
    fastboot getvar variant 2>nul
    echo   Slot count: 
    fastboot getvar slot-count 2>nul
    echo   Active slot: 
    fastboot getvar current-slot 2>nul
    echo   HW version: 
    fastboot getvar hwversion 2>nul
    echo.
    pause
    goto frp_fastboot
)
if /i "%fb_choice%"=="A" (
    set /p "fb_cmd=  Fastboot command (e.g. erase frp): "
    echo   [*] Running: fastboot !fb_cmd!
    fastboot !fb_cmd! 2>nul
    echo   [OK] Done.
    pause
    goto frp_fastboot
)
if "%fb_choice%"=="0" goto frp_menu
goto frp_fastboot

:: ====================================================================
::                     SECTION 3: BLOATWARE
:: ====================================================================
:bloat_menu
cls
echo.
echo   =======================================================================
echo   ^|                     BLOATWARE REMOVAL TOOLKIT                      ^|
echo   =======================================================================
echo.
echo   [1] Quick Clean         [5] Reinstall/Re-enable
echo   [2] Brand-Specific      [6] Disabled List
echo   [3] Full Clean          [7] User Apps Only
echo   [4] App List            [0] Back
echo.
set /p "bl_choice=  Select [1-7/0]: "
if "%bl_choice%"=="1" goto bloat_quick
if "%bl_choice%"=="2" goto bloat_brand
if "%bl_choice%"=="3" goto bloat_full
if "%bl_choice%"=="4" goto bloat_list
if "%bl_choice%"=="5" goto bloat_reinstall
if "%bl_choice%"=="6" goto bloat_disabled
if "%bl_choice%"=="7" goto bloat_user
if "%bl_choice%"=="0" goto main_menu
goto bloat_menu

:bloat_quick
cls
echo.
echo   === QUICK BLOATWARE CLEAN ===
echo   Scanning...
adb shell pm list packages > "%TEMP%\bnt\packages.txt" 2>nul
set /a FOUND=0
set /a REMOVED=0
echo   [1/4] Ad SDKs...
for %%A in (com.startapp.startapp com.applovin com.inmobi com.mopub com.facebook.ads com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.flurry com.chartbeat com.revmob com.nativex com.hyprmx com.verve com.millennialmedia com.chartboost com.leadbolt) do (
    findstr /i "%%A" "%TEMP%\bnt\packages.txt" >nul 2>&1
    if not errorlevel 1 (set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%A 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%A 2>nul)
)
echo   [2/4] Google tracking...
for %%G in (com.google.android.gms.ads.admanager com.google.android.googlequicksearchbox com.google.android.play.games com.google.android.apps.nbu.files com.google.android.apps.youtube.music com.google.android.apps.youtube.kids) do (
    findstr /i "%%G" "%TEMP%\bnt\packages.txt" >nul 2>&1
    if not errorlevel 1 (set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%G 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%G 2>nul)
)
echo   [3/4] Clearing ad data...
for %%C in (com.google.android.gms com.google.android.gms.ads com.facebook.katana) do adb shell pm clear %%C 2>nul
echo   [4/4] Done.
del "%TEMP%\bnt\packages.txt" 2>nul
echo.
echo   =======================================================================
echo   RESULTS: %FOUND% found, %REMOVED% removed/disabled
echo   =======================================================================
echo [%date% %time%] Quick bloat: %FOUND%/%REMOVED% >> "%LOG%"
pause
goto bloat_menu

:bloat_brand
cls
echo.
echo   === BRAND-SPECIFIC BLOATWARE ===
echo   Detected: !MFG! / !BRAND!
echo.
echo   [1] Samsung    [2] Xiaomi    [3] Huawei    [4] OnePlus/Oppo/Realme
echo   [5] Vivo       [6] Pixel     [7] Sony      [8] Motorola/Lenovo
echo   [9] Nokia      [A] LG        [B] ASUS      [C] HTC        [D] ZTE
echo   [0] Back
echo.
set /p "bc=  Select: "
if "%bc%"=="0" goto bloat_menu

adb shell pm list packages > "%TEMP%\bnt\packages.txt" 2>nul
set /a FOUND=0
set /a REMOVED=0

if "%bc%"=="1" (for %%S in (com.sec.android.app.sbrowser com.samsung.android.bixby.agent com.samsung.android.bixby.service com.samsung.android.themestore com.samsung.android.spay com.samsung.android.aremoji com.samsung.android.forest com.samsung.android.samsungpass com.sec.spp.push com.samsung.android.dqagent com.sec.android.widgetapp.samsungweather com.samsung.android.allshare com.samsung.android.helphub com.samsung.android.game.gamehome com.samsung.android.game.gametools com.samsung.android.app.tips com.samsung.android.mobileservice com.samsung.android.visionintelligence com.samsung.android.ardrawing com.samsung.android.arzone com.samsung.android.app.routines com.samsung.android.app.sharelive com.samsung.android.kidsinstaller com.samsung.android.app.splanet) do (findstr /i "%%S" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%S 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%S 2>nul))
if "%bc%"=="2" (for %%X in (com.miui.ad com.miui.analytics com.miui.msa.global com.xiaomi.shop com.xiaomi.joyose com.miui.cleanmaster com.miui.securitycenter com.xiaomi.gamecenter com.xiaomi.market com.xiaomi.xmsf com.miui.mipicks com.miui.huanji com.miui.phonemanager com.miui.cleaner) do (findstr /i "%%X" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%X 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%X 2>nul))
if "%bc%"=="3" (for %%H in (com.huawei.systemmanager com.huawei.hianalytics com.huawei.ads com.huawei.trustagent com.huawei.gamebox.service com.huawei.health com.huawei.smarthome com.huawei.intelligent com.huawei.android.mirror com.huawei.android.projector com.huawei.hmos.weather) do (findstr /i "%%H" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%H 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%H 2>nul))
if "%bc%"=="4" (for %%O in (com.heytap.browser com.heytap.market com.heytap.cloud com.coloros.assistantscreen com.oppo.launcher com.oppo.ota com.realme.hotspot com.oplus.market com.coloros.game com.heytap.usercenter com.coloros.oshare) do (findstr /i "%%O" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%O 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%O 2>nul))
if "%bc%"=="5" (for %%V in (com.bbk.browser com.bbk.cloud com.vivo.weather com.vivo.game com.vivo.health com.iqoo.gamecenter com.bbk.updateservice com.vivo.easyshare com.vivo.market com.vivo.daemon com.vivo.imanager) do (findstr /i "%%V" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%V 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%V 2>nul))
if "%bc%"=="6" (for %%G in (com.google.android.apps.nbu.files com.google.android.apps.chromecast.app com.google.android.apps.youtube.music com.google.android.apps.youtube.kids com.google.android.apps.podcasts com.google.android.apps.magazines com.google.android.apps.books com.google.android.googlequicksearchbox com.google.android.keep com.google.android.apps.fitness com.google.android.apps.tachyon com.google.android.apps.wallpaper com.google.android.apps.wellbeing) do (findstr /i "%%G" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%G 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%G 2>nul))
if "%bc%"=="7" (for %%S in (com.sonyericsson.music com.sonyericsson.video com.sonyericsson.album com.sony.mobileconnected com.sonyericsson.updatecenter) do (findstr /i "%%S" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%S 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%S 2>nul))
if "%bc%"=="8" (for %%M in (com.motorola.genie com.motorola.ccc com.lenovo.anyshare.gps com.lenovo.launcher com.lenovo.music com.lenovo.video com.lenovo.weather com.lenovo.powermanager) do (findstr /i "%%M" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%M 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%M 2>nul))
if "%bc%"=="9" (for %%N in (com.nokia.community com.nokia.support com.nokia.battery) do (findstr /i "%%N" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%N 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%N 2>nul))
if /i "%bc%"=="A" (for %%L in (com.lge.bnr com.lge.gallery com.lge.lgaccount com.lge.lgdm com.lge.music com.lge.remotecontrol com.lge.theme) do (findstr /i "%%L" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%L 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%L 2>nul))
if /i "%bc%"=="B" (for %%A in (com.asus.anycut com.asus.appinstaller com.asus.gallery com.asus.music com.asus.notes com.asus.weather com.asus.webstorage) do (findstr /i "%%A" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%A 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%A 2>nul))
if /i "%bc%"=="C" (for %%T in (com.htc.launcher com.htc.music com.htc.newsreader com.htc.sense com.htc.weather com.htc.widget) do (findstr /i "%%T" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%T 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%T 2>nul))
if /i "%bc%"=="D" (for %%Z in (com.zte.miprogram com.zte.music com.zte.launcher com.nubia.weather com.nubia.gallery com.nubia.calculator) do (findstr /i "%%Z" "%TEMP%\bnt\packages.txt" >nul 2>&1 && set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%Z 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%Z 2>nul))

del "%TEMP%\bnt\packages.txt" 2>nul
echo.
echo   =======================================================================
echo   BRAND RESULTS: %FOUND% found, %REMOVED% removed/disabled
echo   =======================================================================
pause
goto bloat_menu

:bloat_full
cls
echo.
echo   === FULL BLOATWARE CLEAN - ALL BRANDS ===
set /p "confirm=  Run full clean? (Y/N): "
if /i not "%confirm%"=="Y" goto bloat_menu
adb shell pm list packages > "%TEMP%\bnt\packages.txt" 2>nul
set /a FOUND=0
set /a REMOVED=0
echo   [1/4] Ad SDKs...
for %%A in (com.startapp.startapp com.applovin com.inmobi com.mopub com.facebook.ads com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.flurry com.chartbeat com.revmob com.chartboost com.leadbolt) do (
    findstr /i "%%A" "%TEMP%\bnt\packages.txt" >nul 2>&1
    if not errorlevel 1 (set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%A 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%A 2>nul)
)
echo   [2/4] Google...
for %%G in (com.google.android.gms.ads.admanager com.google.android.googlequicksearchbox com.google.android.play.games com.google.android.apps.youtube.music com.google.android.apps.youtube.kids) do (
    findstr /i "%%G" "%TEMP%\bnt\packages.txt" >nul 2>&1
    if not errorlevel 1 (set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%G 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%G 2>nul)
)
echo   [3/4] All brands...
for %%P in (com.sec.android.app.sbrowser com.samsung.android.bixby.agent com.samsung.android.themestore com.samsung.android.spay com.miui.ad com.miui.analytics com.xiaomi.shop com.miui.securitycenter com.xiaomi.market com.xiaomi.xmsf com.huawei.systemmanager com.huawei.hianalytics com.huawei.ads com.heytap.browser com.heytap.market com.oppo.launcher com.realme.hotspot com.bbk.browser com.vivo.weather com.vivo.game com.motorola.genie com.lenovo.anyshare.gps com.nokia.community com.lge.lgaccount) do (
    findstr /i "%%P" "%TEMP%\bnt\packages.txt" >nul 2>&1
    if not errorlevel 1 (set /a FOUND+=1 & adb shell pm uninstall -k --user 0 %%P 2>nul | findstr /i "Success" >nul 2>&1 && set /a REMOVED+=1 || adb shell pm disable-user --user 0 %%P 2>nul)
)
echo   [4/4] Clearing data...
for %%C in (com.google.android.gms com.facebook.katana) do adb shell pm clear %%C 2>nul
del "%TEMP%\bnt\packages.txt" 2>nul
echo.
echo   FULL CLEAN: %FOUND% found, %REMOVED% removed/disabled
echo [%date% %time%] Full bloat: %FOUND%/%REMOVED% >> "%LOG%"
pause
goto bloat_menu

:bloat_list
cls
echo.
echo   [1] All   [2] With paths   [3] Search   [4] System   [0] Back
set /p "lc=  Select: "
if "%lc%"=="1" (echo. & adb shell pm list packages 2>nul)
if "%lc%"=="2" (echo. & adb shell pm list packages -f 2>nul)
if "%lc%"=="3" (set /p "st=  Search: " & echo. & adb shell pm list packages 2>nul | findstr /i "!st!")
if "%lc%"=="4" (echo. & adb shell pm list packages -s 2>nul)
if "%lc%"=="0" goto bloat_menu
echo.
echo   Total: & adb shell pm list packages 2>nul | find /c /v ""
echo.
pause
goto bloat_list

:bloat_reinstall
cls
echo.
echo   [1] Re-enable ALL disabled   [2] Specific   [0] Back
set /p "rc=  Select: "
if "%rc%"=="1" (for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -d 2^>nul') do (adb shell pm enable %%P 2>nul & echo   [ENABLED] %%P))
if "%rc%"=="2" (set /p "ep=  Package: " & adb shell pm enable "!ep!" 2>nul & adb shell pm install-existing "!ep!" 2>nul & echo   [OK] !ep! enabled)
if "%rc%"=="0" goto bloat_menu
echo.
pause
goto bloat_reinstall

:bloat_disabled
cls
echo.
echo   Disabled packages:
adb shell pm list packages -d 2>nul
echo.
pause
goto bloat_menu

:bloat_user
cls
echo.
echo   User-installed packages:
adb shell pm list packages -3 2>nul
echo.
pause
goto bloat_menu

:: ====================================================================
::                     SECTION 4: DEVICE UTILITIES
:: ====================================================================
:utils_menu
cls
echo.
echo   =======================================================================
echo   ^|                     DEVICE UTILITIES                               ^|
echo   =======================================================================
echo.
echo   [1] Full Device Info       [6] Wireless ADB
echo   [2] Battery Status         [7] Backup & Restore
echo   [3] Screenshot             [8] Install APK
echo   [4] Screen Record          [9] File Manager
echo   [5] Reboot Options         [0] Back
echo.
set /p "uc=  Select [1-9/0]: "
if "%uc%"=="1" goto utils_info
if "%uc%"=="2" goto utils_battery
if "%uc%"=="3" goto utils_screenshot
if "%uc%"=="4" goto utils_record
if "%uc%"=="5" goto utils_reboot
if "%uc%"=="6" goto utils_wireless
if "%uc%"=="7" goto utils_backup
if "%uc%"=="8" goto utils_install
if "%uc%"=="9" goto utils_files
if "%uc%"=="0" goto main_menu
goto utils_menu

:utils_info
cls
echo.
echo   === FULL DEVICE INFORMATION ===
echo.
echo   Manufacturer  : !MFG!
echo   Brand         : !BRAND!
echo   Model         : !MODEL!
echo   Device Name   : !DEVNAME!
echo   Device Code   : !DEVICE!
echo   Android       : !ANDROID! (SDK !SDK!)
echo   Build         : !BUILD!
echo   Security Patch: !SECPATCH!
echo   CPU           : !ARCH!
echo   Board         : !BOARD!
echo   Build Type    : !BUILDTYPE!
echo.
echo   --- Hardware ---
for /f "delims=" %%H in ('adb shell cat /proc/cpuinfo 2^>nul ^| findstr /i "Hardware"') do echo   %%H
for /f "delims=" %%H in ('adb shell cat /proc/meminfo 2^>nul ^| findstr "MemTotal"') do echo   %%H
echo.
echo   --- Display ---
for /f "delims=" %%H in ('adb shell wm size 2^>nul') do echo   %%H
for /f "delims=" %%H in ('adb shell wm density 2^>nul') do echo   %%H
echo.
echo   --- Storage ---
for /f "delims=" %%H in ('adb shell df /data 2^>nul ^| findstr "/data"') do echo   %%H
echo.
echo   --- Battery ---
for /f "delims=" %%H in ('adb shell dumpsys battery 2^>nul ^| findstr "level:"') do echo   %%H
for /f "delims=" %%H in ('adb shell dumpsys battery 2^>nul ^| findstr "temperature:"') do echo   %%H
echo.
echo   --- Security ---
echo   Root: !ROOT_STAT!
for /f "delims=" %%H in ('adb shell settings get global adb_enabled 2^>nul') do echo   ADB: %%H
echo.
echo   --- Uptime ---
for /f "delims=" %%H in ('adb shell cat /proc/uptime 2^>nul') do echo   %%H
echo.
pause
goto utils_menu

:utils_battery
cls
echo.
adb shell dumpsys battery 2>nul
echo.
pause
goto utils_menu

:utils_screenshot
cls
echo.
adb shell screencap -p /sdcard/screenshot.png 2>nul
adb pull /sdcard/screenshot.png "%USERPROFILE%\Desktop\screenshot.png" 2>nul
adb shell rm /sdcard/screenshot.png 2>nul
echo   [OK] Saved to Desktop\screenshot.png
echo.
pause
goto utils_menu

:utils_record
cls
echo.
echo   [1] 30s   [2] 60s   [3] 120s   [4] Custom   [0] Back
set /p "rc=  Select: "
set "RT=30"
if "%rc%"=="2" set "RT=60"
if "%rc%"=="3" set "RT=120"
if "%rc%"=="4" set /p "RT=  Seconds: "
if "%rc%"=="0" goto utils_menu
echo   Recording !RT! seconds...
adb shell screenrecord --time-limit !RT! --bit-rate 8000000 /sdcard/recording.mp4 2>nul
adb pull /sdcard/recording.mp4 "%USERPROFILE%\Desktop\recording.mp4" 2>nul
adb shell rm /sdcard/recording.mp4 2>nul
echo   [OK] Saved to Desktop\recording.mp4
echo.
pause
goto utils_menu

:utils_reboot
cls
echo.
echo   [1] Normal   [2] Recovery   [3] Bootloader   [4] Download (Samsung)
echo   [5] Soft (root)   [6] Power Off   [7] Restart SystemUI   [0] Back
set /p "rc=  Select: "
if "%rc%"=="1" (adb reboot & echo   Rebooting...)
if "%rc%"=="2" (adb reboot recovery & echo   Recovery...)
if "%rc%"=="3" (adb reboot bootloader & echo   Bootloader...)
if "%rc%"=="4" (adb reboot download 2>nul & echo   Download...)
if "%rc%"=="5" (if "%ROOTED%"=="1" (adb shell setprop ctl.restart zygote 2>nul & echo   Soft reboot...) else echo   [ERROR] Root required!)
if "%rc%"=="6" (adb shell reboot -p 2>nul & echo   Powering off...)
if "%rc%"=="7" (adb shell am force-stop com.android.systemui 2>nul & echo   SystemUI restarted)
if "%rc%"=="0" goto utils_menu
echo.
pause
goto utils_menu

:utils_wireless
cls
echo.
echo   [1] Enable wireless ADB   [2] Connect by IP   [3] Disconnect   [0] Back
set /p "wc=  Select: "
if "%wc%"=="1" (adb tcpip 5555 2>nul & echo   [OK] Enabled on port 5555 & echo   Device IP: & adb shell ip route 2>nul | findstr "wlan0")
if "%wc%"=="2" (set /p "dip=  IP: " & adb connect !dip!:5555 2>nul & echo   [OK] Connecting)
if "%wc%"=="3" (adb disconnect 2>nul & echo   [OK] Disconnected)
if "%wc%"=="0" goto utils_menu
echo.
pause
goto utils_wireless

:utils_backup
cls
echo.
echo   [1] Backup apps   [2] Contacts   [3] SMS   [0] Back
set /p "bk=  Select: "
if "%bk%"=="1" (
    set /p "bpath=  Folder [%USERPROFILE%\Desktop\AndroidBackup]: "
    if "!bpath!"=="" set "bpath=%USERPROFILE%\Desktop\AndroidBackup"
    if not exist "!bpath!" mkdir "!bpath!"
    for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (
        for /f "tokens=2 delims=:" %%L in ('adb shell pm path %%P 2^>nul ^| findstr "package:"') do adb pull "%%L" "!bpath!\%%P.apk" 2>nul
    )
    echo   [OK] Backed up to !bpath!
)
if "%bk%"=="2" (adb shell content query --uri content://com.android.contacts/contacts 2>nul > "%USERPROFILE%\Desktop\contacts.txt" & echo   [OK] Contacts saved)
if "%bk%"=="3" (adb shell content query --uri content://sms 2>nul > "%USERPROFILE%\Desktop\sms.txt" & echo   [OK] SMS saved)
if "%bk%"=="0" goto utils_menu
echo.
pause
goto utils_backup

:utils_install
cls
echo.
set /p "apk=  APK path: "
if not exist "!apk!" (echo   [ERROR] Not found! & pause & goto utils_install)
adb install "!apk!" 2>nul
echo   [OK] Done.
echo.
pause
goto utils_menu

:utils_files
cls
echo.
echo   [1] Push   [2] Pull   [3] List /sdcard/   [4] Browse   [0] Back
set /p "fc=  Select: "
if "%fc%"=="1" (set /p "lf=  Local: " & set /p "rd=  Dest [/sdcard/]: " & if "!rd!"=="" set "rd=/sdcard/" & adb push "!lf!" "!rd!" 2>nul & echo   [OK])
if "%fc%"=="2" (set /p "rf=  Device file: " & adb pull "!rf!" "%USERPROFILE%\Desktop\" 2>nul & echo   [OK])
if "%fc%"=="3" (echo. & adb shell ls -la /sdcard/ 2>nul & echo.)
if "%fc%"=="4" (set /p "bp=  Path [/sdcard/]: " & if "!bp!"=="" set "bp=/sdcard/" & echo. & adb shell ls -la "!bp!" 2>nul & echo.)
if "%fc%"=="0" goto utils_menu
echo.
pause
goto utils_files

:: ====================================================================
::                     SECTION 5: PRIVACY SHIELD
:: ====================================================================
:privacy_menu
cls
echo.
echo   =======================================================================
echo   ^|                     PRIVACY SHIELD                                 ^|
echo   =======================================================================
echo.
echo   [1] Revoke Permissions      [4] Privacy Audit
echo   [2] Disable Telemetry       [5] Encrypt Data
echo   [3] Block Network Access    [0] Back
echo.
set /p "pc=  Select [1-5/0]: "
if "%pc%"=="1" goto priv_perms
if "%pc%"=="2" goto priv_telem
if "%pc%"=="3" goto priv_network
if "%pc%"=="4" goto priv_audit
if "%pc%"=="5" goto priv_encrypt
if "%pc%"=="0" goto main_menu
goto privacy_menu

:priv_perms
cls
echo.
echo   Revoking from all user apps...
echo   [1/5] Camera...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do adb shell pm revoke %%P android.permission.CAMERA 2>nul
echo   [2/5] Microphone...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do adb shell pm revoke %%P android.permission.RECORD_AUDIO 2>nul
echo   [3/5] Location...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul & adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul)
echo   [4/5] Contacts...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do adb shell pm revoke %%P android.permission.READ_CONTACTS 2>nul
echo   [5/5] SMS...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell pm revoke %%P android.permission.READ_SMS 2>nul & adb shell pm revoke %%P android.permission.SEND_SMS 2>nul)
echo.
echo   [OK] Permissions revoked.
pause
goto privacy_menu

:priv_telem
cls
echo.
echo   [1/6] Usage stats...
adb shell appops set com.google.android.gms USAGE_STATS deny 2>nul
adb shell appops set com.google.android.gms READ_PHONE_STATE deny 2>nul
echo   [2/6] Analytics...
adb shell settings put secure usage_metrics_reporting_enabled 0 2>nul
adb shell settings put secure analytics_enabled 0 2>nul
echo   [3/6] Diagnostics...
adb shell settings put secure send_action_app_error 0 2>nul
echo   [4/6] Usage stats collection...
adb shell settings put secure usage_stats_enabled 0 2>nul
echo   [5/6] Clearing...
adb shell rm -rf /data/system/usagestats/* 2>nul
echo   [6/6] Done.
echo.
echo   [OK] Telemetry disabled.
pause
goto privacy_menu

:priv_network
cls
echo.
echo   [1] Block app network   [2] Airplane ON   [3] Airplane OFF   [0] Back
set /p "nc=  Select: "
if "%nc%"=="1" (set /p "np=  Package: " & adb shell appops set !np! RUN_IN_BACKGROUND deny 2>nul & adb shell appops set !np! RUN_ANY_IN_BACKGROUND deny 2>nul & echo   [OK] !np! restricted)
if "%nc%"=="2" (adb shell settings put global airplane_mode_on 1 2>nul & adb shell am broadcast -a android.intent.action.AIRPLANE_MODE --ez state true 2>nul & echo   [OK] ON)
if "%nc%"=="3" (adb shell settings put global airplane_mode_on 0 2>nul & adb shell am broadcast -a android.intent.action.AIRPLANE_MODE --ez state false 2>nul & echo   [OK] OFF)
if "%nc%"=="0" goto privacy_menu
echo.
pause
goto priv_network

:priv_audit
cls
echo.
echo   === PRIVACY AUDIT ===
echo.
echo   --- CAMERA ---
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell dumpsys package %%P 2>nul | findstr "android.permission.CAMERA" >nul 2>&1 && echo     [CAMERA] %%P)
echo.
echo   --- MICROPHONE ---
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell dumpsys package %%P 2>nul | findstr "android.permission.RECORD_AUDIO" >nul 2>&1 && echo     [MIC] %%P)
echo.
echo   --- LOCATION ---
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell dumpsys package %%P 2>nul | findstr "android.permission.ACCESS_FINE_LOCATION" >nul 2>&1 && echo     [LOCATION] %%P)
echo.
echo   --- PHONE ---
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell dumpsys package %%P 2>nul | findstr "android.permission.READ_PHONE_STATE" >nul 2>&1 && echo     [PHONE] %%P)
echo.
echo   --- SMS ---
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do (adb shell dumpsys package %%P 2>nul | findstr "android.permission.READ_SMS" >nul 2>&1 && echo     [SMS] %%P)
echo.
pause
goto privacy_menu

:priv_encrypt
cls
echo.
echo   [1] Check status   [2] Enable   [0] Back
set /p "ec=  Select: "
if "%ec%"=="1" (echo. & adb shell getprop ro.crypto.state 2>nul & echo. & adb shell getprop vold.decrypt 2>nul)
if "%ec%"=="2" (adb shell vdc cryptfs enablecrypto inplace 2>nul & echo   [OK] Encryption sent. May reboot.)
if "%ec%"=="0" goto privacy_menu
echo.
pause
goto priv_encrypt

:: ====================================================================
::                     SECTION 6: APP MANAGER
:: ====================================================================
:app_menu
cls
echo.
echo   =======================================================================
echo   ^|                     APP MANAGER                                    ^|
echo   =======================================================================
echo.
echo   [1] Force Stop       [4] Enable App
echo   [2] Clear Data       [5] Uninstall App
echo   [3] Disable App      [6] App Info
echo   [7] Bulk Operations  [0] Back
echo.
set /p "ac=  Select [1-7/0]: "
if "%ac%"=="1" goto app_force
if "%ac%"=="2" goto app_clear
if "%ac%"=="3" goto app_disable
if "%ac%"=="4" goto app_enable
if "%ac%"=="5" goto app_uninstall
if "%ac%"=="6" goto app_info
if "%ac%"=="7" goto app_bulk
if "%ac%"=="0" goto main_menu
goto app_menu

:app_force
cls
echo.
set /p "fp=  Package: "
adb shell am force-stop !fp! 2>nul
echo   [OK] !fp! stopped.
echo.
pause
goto app_menu

:app_clear
cls
echo.
set /p "cp=  Package: "
echo   [1] Data   [2] Cache   [3] Both
set /p "ct=  Select: "
if "%ct%"=="1" (adb shell pm clear !cp! 2>nul & echo   [OK] Data cleared)
if "%ct%"=="2" (adb shell pm clear-cache !cp! 2>nul & echo   [OK] Cache cleared)
if "%ct%"=="3" (adb shell pm clear !cp! 2>nul & echo   [OK] All cleared)
echo.
pause
goto app_menu

:app_disable
cls
echo.
set /p "dp=  Package: "
adb shell pm disable-user --user 0 !dp! 2>nul
echo   [OK] !dp! disabled.
echo.
pause
goto app_menu

:app_enable
cls
echo.
set /p "ep=  Package: "
adb shell pm enable !ep! 2>nul
echo   [OK] !ep! enabled.
echo.
pause
goto app_menu

:app_uninstall
cls
echo.
set /p "up=  Package: "
echo   [1] User only (safe)   [2] Complete
set /p "ut=  Select: "
if "%ut%"=="1" (adb shell pm uninstall -k --user 0 !up! 2>nul & echo   [OK] Removed for user)
if "%ut%"=="2" (adb shell pm uninstall !up! 2>nul & echo   [OK] Uninstalled)
echo.
pause
goto app_menu

:app_info
cls
echo.
set /p "ip=  Package: "
echo.
echo   -------------------------------------------------------
adb shell dumpsys package !ip! 2>nul | findstr "versionName"
echo.
adb shell du -sh /data/data/!ip! 2>nul
echo.
adb shell dumpsys package !ip! 2>nul | findstr "pkgFlags"
echo   -------------------------------------------------------
echo.
pause
goto app_menu

:app_bulk
cls
echo.
echo   [1] Disable ad SDKs   [2] Clear all caches
echo   [3] Force-stop users   [4] Remove disabled   [0] Back
set /p "bc=  Select: "
if "%bc%"=="1" (for %%P in (com.applovin com.inmobi com.mopub com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.flurry com.facebook.ads com.startapp) do adb shell pm disable-user --user 0 %%P 2>nul & echo   [OK])
if "%bc%"=="2" (for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages 2^>nul') do adb shell pm clear-cache %%P 2>nul & echo   [OK])
if "%bc%"=="3" (for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do adb shell am force-stop %%P 2>nul & echo   [OK])
if "%bc%"=="4" (for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -d 2^>nul') do (adb shell pm uninstall -k --user 0 %%P 2>nul & echo   [REMOVED] %%P))
if "%bc%"=="0" goto app_menu
echo.
pause
goto app_menu

:: ====================================================================
::                     SECTION 7: QUICK ACTIONS
:: ====================================================================
:quick_menu
cls
echo.
echo   =======================================================================
echo   ^|                     QUICK ACTIONS                                  ^|
echo   =======================================================================
echo.
echo   [1] One-Tap Optimize       [5] Disable Notifications
echo   [2] Clear All Cache        [6] Screen Timeout
echo   [3] Screenshot             [7] USB Configuration
echo   [4] Wake Device            [8] Battery Saver
echo   [0] Back
echo.
set /p "qc=  Select [1-8/0]: "
if "%qc%"=="1" goto quick_opt
if "%qc%"=="2" goto quick_cache
if "%qc%"=="3" adb shell screencap -p /sdcard/screenshot.png 2>nul & adb pull /sdcard/screenshot.png "%USERPROFILE%\Desktop\screenshot.png" 2>nul & adb shell rm /sdcard/screenshot.png 2>nul & echo   [OK] Saved. & pause
if "%qc%"=="4" adb shell input keyevent KEYCODE_WAKEUP 2>nul & timeout /t 1 >nul & adb shell input keyevent 82 2>nul & echo   [OK] Woken. & pause
if "%qc%"=="5" adb shell settings put global heads_up_notifications_enabled 0 2>nul & echo   [OK] Notifications disabled. & pause
if "%qc%"=="6" goto quick_timeout
if "%qc%"=="7" goto quick_usb
if "%qc%"=="8" adb shell settings put global low_power 1 2>nul & echo   [OK] Battery saver ON. & pause
if "%qc%"=="0" goto main_menu
goto quick_menu

:quick_opt
cls
echo.
echo   === ONE-TAP OPTIMIZATION ===
echo   [1/5] Clearing caches...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages 2^>nul') do adb shell pm clear-cache %%P 2>nul
echo   [2/5] Force-stopping apps...
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages -3 2^>nul') do adb shell am force-stop %%P 2>nul
echo   [3/5] Kill stale...
adb shell am kill-all 2>nul
echo   [4/5] Flush DNS...
adb shell cmd connectivity flush-dns 2>nul
echo   [5/5] Free memory...
adb shell "echo 3 > /proc/sys/vm/drop_caches" 2>nul
echo.
echo   [OK] Optimized!
echo.
pause
goto quick_menu

:quick_cache
cls
echo.
for /f "tokens=2 delims=: " %%P in ('adb shell pm list packages 2^>nul') do adb shell pm clear-cache %%P 2>nul
echo   [OK] All caches cleared.
echo.
pause
goto quick_menu

:quick_timeout
cls
echo.
echo   [1] 15s  [2] 30s  [3] 1min  [4] 5min  [5] 10min  [6] Never  [0] Back
set /p "tc=  Select: "
if "%tc%"=="1" adb shell settings put system screen_off_timeout 15000 2>nul
if "%tc%"=="2" adb shell settings put system screen_off_timeout 30000 2>nul
if "%tc%"=="3" adb shell settings put system screen_off_timeout 60000 2>nul
if "%tc%"=="4" adb shell settings put system screen_off_timeout 300000 2>nul
if "%tc%"=="5" adb shell settings put system screen_off_timeout 600000 2>nul
if "%tc%"=="6" adb shell svc power stayon true 2>nul
if "%tc%"=="0" goto quick_menu
echo   [OK] Timeout updated.
echo.
pause
goto quick_menu

:quick_usb
cls
echo.
echo   [1] MTP   [2] PTP   [3] RNDIS   [4] MIDI   [5] Charge Only   [0] Back
set /p "uc=  Select: "
if "%uc%"=="1" adb shell setprop sys.usb.config mtp 2>nul
if "%uc%"=="2" adb shell setprop sys.usb.config ptp 2>nul
if "%uc%"=="3" adb shell setprop sys.usb.config rndis 2>nul
if "%uc%"=="4" adb shell setprop sys.usb.config midi 2>nul
if "%uc%"=="5" adb shell setprop sys.usb.config charge_only 2>nul
if "%uc%"=="0" goto quick_menu
echo   [OK] USB mode updated.
echo.
pause
goto quick_menu

:: ====================================================================
::                     SECTION 8: DEVELOPER TOOLS
:: ====================================================================
:dev_menu
cls
echo.
echo   =======================================================================
echo   ^|                     DEVELOPER TOOLS                                ^|
echo   =======================================================================
echo.
echo   [1] Logcat Real-time     [5] Run Shell Command
echo   [2] Logcat to File       [6] Monkey Test
echo   [3] Dumpsys Info         [7] CPU Benchmark
echo   [4] ADB Shell            [0] Back
echo.
set /p "dc=  Select [1-7/0]: "
if "%dc%"=="1" (echo   Ctrl+C to stop & adb logcat 2>nul)
if "%dc%"=="2" (adb logcat -d > "%USERPROFILE%\Desktop\logcat_%date:~-4%%date:~-7,2%%date:~-10,2%.txt" 2>nul & echo   [OK] Saved.)
if "%dc%"=="3" goto dev_dumpsys
if "%dc%"=="4" (echo   Type "exit" to return & adb shell & echo.)
if "%dc%"=="5" (set /p "cmd=  Command: " & adb !cmd! 2>nul & echo.)
if "%dc%"=="6" (echo   Running 500 events... & adb shell monkey -p com.android.launcher --throttle 500 -v 500 2>nul & echo   [OK] Done.)
if "%dc%"=="7" (echo   CPU: & for /f "delims=" %%H in ('adb shell cat /proc/cpuinfo 2^>nul ^| findstr /i "model name"') do echo   %%H & echo.
    echo   Running dd test... & adb shell "dd if=/dev/zero of=/data/local/tmp/bench bs=1M count=100 2>&1 | tail -1" 2>nul & adb shell rm /data/local/tmp/bench 2>nul)
if "%dc%"=="0" goto main_menu
echo.
pause
goto dev_menu

:dev_dumpsys
cls
echo.
echo   [1] Activity   [2] Window   [3] Battery   [4] Meminfo   [5] CPU   [6] All
set /p "dc=  Select: "
if "%dc%"=="1" adb shell dumpsys activity activities 2>nul
if "%dc%"=="2" adb shell dumpsys window windows 2>nul
if "%dc%"=="3" adb shell dumpsys battery 2>nul
if "%dc%"=="4" adb shell dumpsys meminfo 2>nul
if "%dc%"=="5" adb shell dumpsys cpuinfo 2>nul
if "%dc%"=="6" adb shell dumpsys 2>nul
echo.
pause
goto dev_menu

:: ====================================================================
::                     SECTION 9: NETWORK TOOLS
:: ====================================================================
:net_menu
cls
echo.
echo   =======================================================================
echo   ^|                     NETWORK TOOLS                                  ^|
echo   =======================================================================
echo.
echo   [1] WiFi Info       [4] Ping Test
echo   [2] WiFi Scan       [5] DNS Lookup
echo   [3] IP Config       [0] Back
echo.
set /p "nc=  Select [1-5/0]: "
if "%nc%"=="1" (echo. & adb shell dumpsys wifi 2>nul | findstr "mWifiInfo" & echo. & adb shell ip route 2>nul | findstr "wlan0" & echo.)
if "%nc%"=="2" (echo   Scanning... & adb shell cmd wifi start-scan 2>nul & timeout /t 3 >nul & adb shell cmd wifi list-scan-results 2>nul & echo.)
if "%nc%"=="3" (echo. & adb shell ip addr show 2>nul & echo. & adb shell ip route 2>nul & echo.)
if "%nc%"=="4" (set /p "host=  Host: " & adb shell ping -c 4 !host! 2>nul & echo.)
if "%nc%"=="5" (set /p "host=  Hostname: " & adb shell nslookup !host! 2>nul & echo.)
if "%nc%"=="0" goto main_menu
echo.
pause
goto net_menu

:: ====================================================================
::                     SECTION 0: SETTINGS
:: ====================================================================
:settings_menu
cls
echo.
echo   =======================================================================
echo   ^|                     TOOL SETTINGS                                  ^|
echo   =======================================================================
echo.
echo   [1] View Tool Log          [3] About
echo   [2] Export Device Report   [4] Help
echo   [0] Back to Dashboard
echo.
set /p "sc=  Select [1-4/0]: "
if "%sc%"=="1" (cls & echo. & if exist "%LOG%" (type "%LOG%") else (echo   No log.) & echo. & pause)
if "%sc%"=="2" goto sett_export
if "%sc%"=="3" (cls & echo. & echo   BNT ANDROID TOOLS DASHBOARD v7.0 & echo   Created by BNTWORX & echo. & echo   Features: & echo   - Ad Removal (hosts, DNS, SDKs, nuclear) & echo   - FRP Bypass (12 methods) & echo   - Bloatware Removal (13 brands) & echo   - Device Utilities, Privacy Shield & echo   - App Manager, Quick Actions & echo   - Developer Tools, Network Tools & echo. & echo   Requirements: & echo   - ADB ^| USB Debugging enabled & echo   - https://developer.android.com/tools/releases/platform-tools & echo. & pause)
if "%sc%"=="4" (cls & echo. & echo   1. Install ADB from Android SDK Platform Tools & echo   2. Enable USB Debugging & echo      Settings ^> About Phone ^> Tap Build Number 7x & echo      Settings ^> Developer Options ^> USB Debugging ON & echo   3. Connect USB, accept RSA prompt & echo   4. Run this tool & echo. & echo   ROOT: Hosts file ^| Soft reboot & echo   FRP: Try option 7 (Full Bypass) first & echo   ADS: Use option 7 (Full Clean) for best results & echo. & echo   TROUBLESHOOTING: & echo   - Device not detected? Check cable + USB debugging & echo   - adb kill-server ^&^& adb start-server & echo. & echo   v7.0 by BNTWORX & echo. & pause)
if "%sc%"=="0" goto main_menu
goto settings_menu

:sett_export
cls
echo.
echo   Generating report...
(
echo BNT Android Tools - Device Report
echo Generated: %date% %time%
echo ================================================
echo MANUFACTURER: !MFG!
echo BRAND: !BRAND!
echo MODEL: !MODEL!
echo DEVICE: !DEVICE!
echo ANDROID: !ANDROID!
echo SDK: !SDK!
echo BUILD: !BUILD!
echo SECURITY PATCH: !SECPATCH!
echo ROOT: !ROOT_STAT!
echo.
echo --- PACKAGES ---
adb shell pm list packages 2>nul
echo.
echo --- DISABLED ---
adb shell pm list packages -d 2>nul
echo.
echo --- BATTERY ---
adb shell dumpsys battery 2>nul
echo.
echo --- STORAGE ---
adb shell df 2>nul
echo.
echo --- PROPERTIES ---
adb shell getprop 2>nul
) > "%USERPROFILE%\Desktop\BNT_Report_%date:~-4%%date:~-7,2%%date:~-10,2%.txt" 2>nul
echo   [OK] Report saved to Desktop.
echo.
pause
goto settings_menu

:: ====================================================================
::                          EXIT
:: ====================================================================
:end
cls
echo.
echo   =======================================================================
echo   ^|                                                                     ^|
echo   ^|              BNT ANDROID TOOLS DASHBOARD v7.0                       ^|
echo   ^|              Thank you for using the tool!                          ^|
echo   ^|              Created by BNTWORX                                     ^|
echo   ^|                                                                     ^|
echo   =======================================================================
echo.
echo [%date% %time%] Session ended >> "%LOG%"
pause
exit /b 0
