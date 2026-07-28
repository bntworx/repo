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
title BNT Android Tools
color 0A

echo ============================================
echo           ANDROID TOOLS v6.0
echo           CREATED BY BNTWORX
echo ============================================
echo.

:: Check ADB
adb version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] ADB not found. Install Android SDK Platform Tools
    echo         and add to PATH.
    echo         https://developer.android.com/tools/releases/platform-tools
    pause
    exit /b 1
)

:: Check device
echo [*] Checking for connected device...
adb devices | findstr /r "device$" >nul
if errorlevel 1 (
    echo [ERROR] No device found.
    echo         1. Enable USB Debugging on your phone
    echo         2. Connect via USB
    echo         3. Accept the RSA prompt on phone
    pause
    exit /b 1
)
echo [OK] Device connected.
echo.

:: Detect phone model
echo [*] Detecting phone model...

adb shell getprop ro.product.manufacturer > "%TEMP%\adb_mfg.txt" 2>nul
adb shell getprop ro.product.model > "%TEMP%\adb_model.txt" 2>nul
adb shell getprop ro.build.version.release > "%TEMP%\adb_android.txt" 2>nul
adb shell getprop ro.build.display.id > "%TEMP%\adb_build.txt" 2>nul

set "MFG="
set "MODEL="
set "ANDROID="
set "BUILD="
set /p MFG=<"%TEMP%\adb_mfg.txt" 2>nul
set /p MODEL=<"%TEMP%\adb_model.txt" 2>nul
set /p ANDROID=<"%TEMP%\adb_android.txt" 2>nul
set /p BUILD=<"%TEMP%\adb_build.txt" 2>nul
del "%TEMP%\adb_mfg.txt" "%TEMP%\adb_model.txt" "%TEMP%\adb_android.txt" "%TEMP%\adb_build.txt" 2>nul

echo.
echo   Phone Info:
echo   ---------------------------------
echo   Manufacturer : %MFG%
echo   Model        : %MODEL%
echo   Android Ver  : %ANDROID%
echo   Build ID     : %BUILD%
echo   ---------------------------------
echo.

:: Check root
echo [*] Checking root access...
adb shell su -c "id" 2>nul | findstr "uid=0" >nul
if errorlevel 1 (
    echo [WARNING] Device may not be rooted. Some features need root.
    echo           Trying non-root methods instead...
    set "ROOTED=0"
) else (
    echo [OK] Root access confirmed.
    set "ROOTED=1"
)
echo.

:main_menu
echo ============================================
echo        ANDROID TOOLS v6.0
echo        Created by BNTworx
echo ============================================
echo.
echo   1. Block ads via hosts file (Root required)
echo   2. Remove bloatware (No root needed)
echo   3. Disable ad services (No root needed)
echo   4. FRP Bypass tools (No root needed)
echo   5. All of the above
echo   6. Exit
echo.
set /p "choice=  Enter choice [1-6]: "

if "%choice%"=="1" goto hosts
if "%choice%"=="2" goto bloatware
if "%choice%"=="3" goto services
if "%choice%"=="4" goto frp
if "%choice%"=="5" goto all
if "%choice%"=="6" goto end
echo [ERROR] Invalid choice.
goto main_menu

:hosts
echo.
echo [*] Setting up ad-blocking hosts file...
if "%ROOTED%"=="0" (
    echo [ERROR] Root required for hosts file modification.
    goto menu_loop
)
adb shell su -c "mount -o rw,remount /system" 2>nul
adb shell su -c "cp /system/etc/hosts /system/etc/hosts.bak" 2>nul

echo [*] Pushing ad-blocking hosts...
(
echo 127.0.0.1 localhost
echo 127.0.0.1 ad.doubleclick.net
echo 127.0.0.1 pagead2.googlesyndication.com
echo 127.0.0.1 adservice.google.com
echo 127.0.0.1 googleads.g.doubleclick.net
echo 127.0.0.1 www.googleadservices.com
echo 127.0.0.1 ad.turn.com
echo 127.0.0.1 ads.mopub.com
echo 127.0.0.1 ads.yahoo.com
echo 127.0.0.1 ad.yieldmanager.com
echo 127.0.0.1 adobedtm.com
echo 127.0.0.1 amazon-adsystem.com
echo 127.0.0.1 analytics.google.com
echo 127.0.0.1 app-measurement.com
echo 127.0.0.1 chartbeat.net
echo 127.0.0.1 doubleclick.net
echo 127.0.0.1 fls.doubleclick.net
echo 127.0.0.1 google-analytics.com
echo 127.0.0.1 googletagmanager.com
echo 127.0.0.1 inmobi.com
echo 127.0.0.1 kochava.com
echo 127.0.0.1 leanplum.com
echo 127.0.0.1 localytics.com
echo 127.0.0.1 moatads.com
echo 127.0.0.1 mopub.com
echo 127.0.0.1 openx.net
echo 127.0.0.1 outbrain.com
echo 127.0.0.1 quantserve.com
echo 127.0.0.1 revcontent.com
echo 127.0.0.1 rubiconproject.com
echo 127.0.0.1 scorecardresearch.com
echo 127.0.0.1 taboola.com
echo 127.0.0.1 tapjoy.com
echo 127.0.0.1 unity3d.com
echo 127.0.0.1 vungle.com
echo 127.0.0.1 zedo.com
echo 127.0.0.1 media.net
echo 127.0.0.1 adnxs.com
echo 127.0.0.1 casalemedia.com
echo 127.0.0.1 demdex.net
echo 127.0.0.1 pubmatic.com
echo 127.0.0.1 adcolony.com
echo 127.0.0.1 airpush.com
) > "%TEMP%\hosts_adblock"

adb push "%TEMP%\hosts_adblock" /sdcard/hosts_adblock
adb shell su -c "cp /sdcard/hosts_adblock /system/etc/hosts"
adb shell su -c "chmod 644 /system/etc/hosts"
adb shell su -c "mount -o ro,remount /system"
del "%TEMP%\hosts_adblock"
echo [OK] Hosts file updated with ad domains blocked.
echo.
goto menu_loop

:bloatware
echo.
echo [*] Removing bloatware (no root needed)...
echo.

echo   Scanning installed packages...
adb shell pm list packages > "%TEMP%\adb_packages.txt" 2>nul

set /a FOUND=0
set /a REMOVED=0

echo.
echo   [1/8] Ad SDKs...
for %%A in (com.startapp.startapp com.applovin com.applovin.applovin com.inmobi com.inmobi.analytics com.mopub com.mopub.ads com.facebook.ads com.facebook.ads.internal com.unity3d.services com.unity3d.services.ads com.adcolony com.adcolony.sdk com.tapjoy com.tapjoy.sdk com.vungle com.vungle.ads com.fyber com.fyber.insights com.yieldmo com.braze com.braze.sdk com.localytics com.urbanairship com.onesignal com.onesignal.onesignalSDK com.pushwoosh com.pushwoosh.sdk com.kochava com.kochava.analytics com.appsflyer com.appsflyer.sdk com.adjust com.adjust.sdk com.ironsource com.ironsource.sdk com.smaato com.smaato.sdk com.chartbeat com.chartbeat.androidsdk com.flurry com.flurry.android com.segment com.segment.analytics-sdk-android com.revmob com.nativex com.hyprmx com.verve com.millennialmedia com.chartboost com.leadbolt) do (
    findstr /i "%%A" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%A
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%A 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%A 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [2/8] Samsung bloatware...
for %%S in (com.sec.android.app.sbrowser com.samsung.android.app.spage com.samsung.android.bixby.agent com.samsung.android.bixby.service com.samsung.android.bixby.voice com.samsung.android.visionintelligence com.samsung.android.game.gamehome com.samsung.android.game.gametools com.samsung.android.app.tips com.samsung.android.mobileservice com.samsung.android.themestore com.samsung.android.spay com.samsung.android.aremoji com.samsung.android.ardrawing com.samsung.android.arzone com.samsung.android.app.routines com.samsung.android.forest com.samsung.android.legalparser com.samsung.android.rubin.app com.samsung.android.samsungpass com.samsung.android.app.sharelive com.samsung.android.kidsinstaller com.samsung.android.app.splanet com.sec.spp.push com.samsung.android.dqagent com.sec.android.widgetapp.samsungweather com.samsung.android.allshare com.samsung.android.helphub) do (
    findstr /i "%%S" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%S
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%S 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%S 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [3/8] Xiaomi/MIUI bloatware...
for %%X in (com.miui.ad com.miui.analytics com.miui.msa.global com.xiaomi.shop com.xiaomi.joyose com.miui.cleanmaster com.miui.securitycenter com.miui.daemon com.miui.bugreport com.miui.misound com.miui.screenrecorder com.miui.player com.xiaomi.gamecenter com.xiaomi.gamecenter.sdk.service com.xiaomi.market com.xiaomi.xmsf com.xiaomi.smarthome com.xiaomi.finddevice com.milink.service com.xiaomi.midrop com.miui.yellowpage com.miui.contentcatcher com.miui.carlink com.miui.accessibility com.xiaomi.scanner com.xiaomi.channel) do (
    findstr /i "%%X" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%X
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%X 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%X 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [4/8] Huawei bloatware...
for %%H in (com.huawei.systemmanager com.huawei.android.hsf com.huawei.hwid com.huawei.hianalytics com.huawei.ads com.huawei.trustagent com.huawei.watch.system.service com.huawei.gamebox.service com.huawei.health com.huawei.music com.huawei.videoplayer com.huawei.smarthome com.huawei.intelligent com.huawei.hmos.weather com.huawei.android.mirror com.huawei.android.projector) do (
    findstr /i "%%H" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%H
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%H 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%H 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [5/8] OnePlus/Oppo/Realme bloatware...
for %%O in (com.heytap.browser com.heytap.market com.heytap.cloud com.heytap.htms com.heytap.themestore com.coloros.assistantscreen com.coloros.weather2 com.coloros.musicplay com.coloros.video com.oppo.launcher com.oppo.ota com.oppo.market com.realme.hotspot com.realme.market com.oplus.market com.coloros.game com.oplus.gamespace) do (
    findstr /i "%%O" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%O
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%O 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%O 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [6/8] Vivo/iQOO bloatware...
for %%V in (com.bbk.browser com.bbk.cloud com.bbk.launcher2 com.vivo.weather com.vivo.weatherb com.vivo.space com.vivo.game com.vivo.health com.vivo.permissionmanager com.iqoo.gamecenter com.bbk.updateservice com.vivo.easyshare) do (
    findstr /i "%%V" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%V
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%V 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%V 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [7/8] Google tracking bloatware...
for %%G in (com.google.android.gms.ads.admanager com.google.android.googlequicksearchbox com.google.android.apps.magazines com.google.android.play.games com.google.android.apps.cloudprint com.google.android.apps.docs com.google.android.apps.books com.google.android.apps.plus com.google.android.apps.nbu.files com.google.android.apps.chromecast.app com.google.android.apps.youtube.music com.google.android.apps.youtube.kids com.google.android.apps.podcasts com.google.android.keep) do (
    findstr /i "%%G" "%TEMP%\adb_packages.txt" >nul 2>&1
    if not errorlevel 1 (
        echo     [FOUND] %%G
        set /a FOUND+=1
        adb shell pm uninstall -k --user 0 %%G 2>nul | findstr /i "Success" >nul 2>&1
        if not errorlevel 1 (
            echo       [REMOVED]
            set /a REMOVED+=1
        ) else (
            adb shell pm disable-user --user 0 %%G 2>nul
            echo       [DISABLED]
        )
    )
)

echo.
echo   [8/8] Clearing leftover ad data...
for %%C in (com.google.android.gms com.google.android.gms.ads com.google.android.gms.ads.admanager com.google.android.gms.analytics com.facebook.katana com.facebook.appmanager) do (
    adb shell pm clear %%C 2>nul
)

del "%TEMP%\adb_packages.txt" 2>nul

echo.
echo ============================================
echo   RESULTS: %FOUND% found, %REMOVED% removed
echo ============================================
echo.
echo [OK] Bloatware removal completed.
echo.
goto menu_loop

:services
echo.
echo [*] Disabling ad services (no root needed)...

echo.
echo   [1/8] Disabling Google ad components...
adb shell pm disable-user --user 0 com.google.android.gms.ads 2>nul
adb shell pm disable-user --user 0 com.google.android.gms.ads.admanager 2>nul
adb shell pm disable-user --user 0 com.google.android.gms.analytics 2>nul
adb shell pm disable-user --user 0 com.google.android.apps.ads.services 2>nul
adb shell pm disable-user --user 0 com.google.android.gms.games 2>nul
adb shell pm disable-user --user 0 com.google.android.googlequicksearchbox 2>nul

echo   [2/8] Disabling ad SDK packages...
for %%P in (com.applovin com.inmobi com.mopub com.unity3d.services com.adcolony com.tapjoy com.vungle com.fyber com.yieldmo com.braze com.localytics com.onesignal com.kochava com.appsflyer com.adjust com.ironsource com.smaato com.millennialmedia com.flurry com.segment com.revmob com.nativex com.hyprmx com.verve) do (
    adb shell pm disable-user --user 0 %%P 2>nul
)

echo   [3/8] Limiting ad tracking...
adb shell settings put secure advertising_id ""
adb shell settings put secure limit_ad_tracking 1

echo   [4/8] Disabling personalized ads settings...
adb shell settings put secure google_ad_id ""
adb shell settings put global ad_id_opt_out 1
adb shell settings put secure ad_id_opt_out 1
adb shell settings put secure interest_based_ad 0
adb shell settings put secure interest_based_ads 0
adb shell settings put secure gd_ad_id ""

echo   [5/8] Clearing ad data from apps...
for %%C in (com.google.android.gms com.google.android.gms.ads com.google.android.gms.analytics com.google.android.apps.ads.services com.facebook.katana com.facebook.appmanager) do (
    adb shell pm clear %%C 2>nul
)

echo   [6/8] Force-stopping ad processes...
for %%F in (com.google.android.gms.ads com.google.android.gms.analytics com.facebook.ads com.applovin com.inmobi com.mopub com.unity3d.services com.adcolony com.tapjoy com.vungle) do (
    adb shell am force-stop %%F 2>nul
)

echo   [7/8] Revoking ad permissions...
for %%P in (com.google.android.gms com.facebook.katana) do (
    adb shell pm revoke %%P android.permission.READ_PHONE_STATE 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_FINE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.ACCESS_COARSE_LOCATION 2>nul
    adb shell pm revoke %%P android.permission.GET_ACCOUNTS 2>nul
)

echo   [8/8] Clearing DNS and ad cache...
adb shell "cmd connectivity flush-dns" 2>nul
adb shell settings put global captive_portal_mode 0 2>nul

echo.
echo [OK] Ad services disabled aggressively.
echo.
goto menu_loop

:frp
echo.
echo [*] FRP Bypass Tools (no root needed)...
echo.
echo   WARNING: Only use on devices you own.
echo   FRP lock activates after factory reset if
echo   a Google account was previously signed in.
echo.

:frp_menu
echo ============================================
echo        FRP BYPASS MENU
echo ============================================
echo.
echo   1. Bypass Setup Wizard
echo   2. Open Settings directly
echo   3. Remove Google account
echo   4. Clear Google Account Manager data
echo   5. Disable FRP lock
echo   6. Launch browser for account recovery
echo   7. Full FRP bypass (all methods)
echo   8. Fastboot FRP Bypass
echo   9. Back to main menu
echo.
set /p "frp_choice=  Enter choice [1-9]: "

if "%frp_choice%"=="1" goto frp_setup
if "%frp_choice%"=="2" goto frp_settings
if "%frp_choice%"=="3" goto frp_account
if "%frp_choice%"=="4" goto frp_clear
if "%frp_choice%"=="5" goto frp_disable
if "%frp_choice%"=="6" goto frp_browser
if "%frp_choice%"=="7" goto frp_all
if "%frp_choice%"=="8" goto frp_fastboot
if "%frp_choice%"=="9" goto main_menu
echo [ERROR] Invalid choice.
goto frp_menu

:frp_setup
echo.
echo [*] Bypassing Setup Wizard...
adb shell settings put global device_provisioned 1 2>nul
adb shell settings put secure user_setup_complete 1 2>nul
adb shell content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1 2>nul
adb shell am start -a android.intent.action.MAIN -c android.intent.category.HOME 2>nul
echo [OK] Setup Wizard bypassed. Device should go to home screen.
echo.
goto frp_loop

:frp_settings
echo.
echo [*] Opening Settings...
adb shell am start com.android.settings/com.android.settings.Settings 2>nul
echo [OK] Settings opened on device.
echo.
goto frp_loop

:frp_account
echo.
echo [*] Removing Google account...
adb shell pm clear com.google.android.gsf.login 2>nul
adb shell pm clear com.google.android.gms 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.auth.authzen 2>nul
adb shell pm clear com.google.android.gms.auth.cryptauth 2>nul
adb shell pm clear com.google.android.gms.trust 2>nul
echo [OK] Google account data cleared.
echo.
goto frp_loop

:frp_clear
echo.
echo [*] Clearing Google Account Manager data...
adb shell pm clear com.google.android.gsf 2>nul
adb shell pm clear com.google.android.gsf.login 2>nul
adb shell pm clear com.google.android.gms 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.auth.authzen 2>nul
adb shell pm clear com.google.android.gms.auth.cryptauth 2>nul
adb shell pm clear com.google.android.gms.trust 2>nul
adb shell pm clear com.google.android.gms.fido 2>nul
adb shell pm clear com.google.android.gms.tapandpay 2>nul
echo [OK] Google Account Manager data cleared.
echo.
goto frp_loop

:frp_disable
echo.
echo [*] Disabling FRP lock...
adb shell settings put secure frp_mode_disabled 1 2>nul
adb shell content insert --uri content://settings/secure --bind name:s:frp_mode_disabled --bind value:s:1 2>nul
adb shell settings put global device_provisioned 1 2>nul
adb shell settings put secure user_setup_complete 1 2>nul
adb shell content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1 2>nul
echo [OK] FRP lock settings disabled.
echo.
goto frp_loop

:frp_browser
echo.
echo [*] Launching browser for account recovery...
adb shell am start -a android.intent.action.VIEW -d "https://accounts.google.com/signin/recovery" 2>nul
echo [OK] Browser opened for Google account recovery.
echo.
goto frp_loop

:frp_all
echo.
echo [*] Running full FRP bypass...
echo.
echo   [1/6] Disabling FRP lock...
adb shell settings put secure frp_mode_disabled 1 2>nul
adb shell content insert --uri content://settings/secure --bind name:s:frp_mode_disabled --bind value:s:1 2>nul

echo   [2/6] Marking device as provisioned...
adb shell settings put global device_provisioned 1 2>nul

echo   [3/6] Completing setup wizard...
adb shell settings put secure user_setup_complete 1 2>nul
adb shell content insert --uri content://settings/secure --bind name:s:user_setup_complete --bind value:s:1 2>nul

echo   [4/6] Clearing Google account data...
adb shell pm clear com.google.android.gsf.login 2>nul
adb shell pm clear com.google.android.gsf 2>nul
adb shell pm clear com.google.android.gms 2>nul
adb shell pm clear com.google.android.gms.auth 2>nul
adb shell pm clear com.google.android.gms.auth.authzen 2>nul
adb shell pm clear com.google.android.gms.auth.cryptauth 2>nul
adb shell pm clear com.google.android.gms.trust 2>nul
adb shell pm clear com.google.android.gms.fido 2>nul

echo   [5/6] Going to home screen...
adb shell am start -a android.intent.action.MAIN -c android.intent.category.HOME 2>nul

echo   [6/6] Opening Settings...
adb shell am start com.android.settings/com.android.settings.Settings 2>nul

echo.
echo [OK] Full FRP bypass completed.
echo       You should now have access to the device.
echo       Open Settings and add your own Google account.
echo.
goto frp_loop

:frp_fastboot
echo.
echo [*] Fastboot FRP Bypass...
echo.
echo   WARNING: Device must be in fastboot mode!
echo   Use: adb reboot bootloader  (to enter fastboot)
echo.
echo   1. Reboot to Bootloader
echo   2. Erase FRP Partition
echo   3. Erase Persist Partition
echo   4. Erase FRP + Persist + Cache + Userdata
echo   5. OEM Unlock
echo   6. Unlock Bootloader
echo   7. Reboot to Recovery
echo   8. Reboot System
echo   9. Custom Fastboot Command
echo   10. Back
echo.
set /p "fb_choice=  Enter choice [1-10]: "

if "%fb_choice%"=="1" (
    echo [*] Rebooting to bootloader...
    adb reboot bootloader 2>nul
    echo [OK] Device should be in fastboot mode.
)
if "%fb_choice%"=="2" (
    set /p "confirm=  Erase FRP partition? (Y/N): "
    if /i "%confirm%"=="Y" (
        echo [*] Erasing FRP partition...
        fastboot erase frp 2>nul
        echo [*] Rebooting...
        fastboot reboot 2>nul
        echo [OK] FRP partition erased.
    )
)
if "%fb_choice%"=="3" (
    set /p "confirm=  Erase persist partition? (Y/N): "
    if /i "%confirm%"=="Y" (
        echo [*] Erasing persist partition...
        fastboot erase persist 2>nul
        echo [*] Rebooting...
        fastboot reboot 2>nul
        echo [OK] Persist partition erased.
    )
)
if "%fb_choice%"=="4" (
    set /p "confirm=  Erase ALL? (Y/N): "
    if /i "%confirm%"=="Y" (
        echo [1/5] Erasing FRP...
        fastboot erase frp 2>nul
        echo [2/5] Erasing persist...
        fastboot erase persist 2>nul
        echo [3/5] Erasing cache...
        fastboot erase cache 2>nul
        echo [4/5] Erasing userdata...
        fastboot erase userdata 2>nul
        echo [5/5] Rebooting...
        fastboot reboot 2>nul
        echo [OK] All partitions erased.
    )
)
if "%fb_choice%"=="5" (
    echo [1/2] OEM unlock...
    fastboot oem unlock 2>nul
    echo [2/2] Flashing unlock...
    fastboot flashing unlock 2>nul
    echo [OK] OEM unlock commands sent.
)
if "%fb_choice%"=="6" (
    set /p "confirm=  Unlock bootloader? DATA WIPE! (Y/N): "
    if /i "%confirm%"=="Y" (
        echo [1/3] OEM unlock...
        fastboot oem unlock 2>nul
        echo [2/3] Flashing unlock...
        fastboot flashing unlock 2>nul
        echo [3/3] Flashing unlock_critical...
        fastboot flashing unlock_critical 2>nul
        echo [OK] Bootloader unlock commands sent.
    )
)
if "%fb_choice%"=="7" (
    echo [*] Rebooting to recovery...
    fastboot reboot recovery 2>nul
    echo [OK] Done.
)
if "%fb_choice%"=="8" (
    echo [*] Rebooting to system...
    fastboot reboot 2>nul
    echo [OK] Done.
)
if "%fb_choice%"=="9" (
    set /p "fb_cmd=  Fastboot command: "
    echo [*] Running: fastboot %fb_cmd%
    fastboot %fb_cmd% 2>nul
    echo [OK] Done.
)
if "%fb_choice%"=="10" goto frp_loop
echo.
goto frp_loop

:frp_loop
echo ============================================
echo   FRP Tool completed. What next?
echo ============================================
echo.
echo   1. Return to FRP menu
echo   2. Return to main menu
echo   3. Reboot device
echo   4. Exit
echo.
set /p "frp_again=  Enter choice [1-4]: "
if "%frp_again%"=="1" goto frp_menu
if "%frp_again%"=="2" goto main_menu
if "%frp_again%"=="3" (
    echo [*] Rebooting device...
    adb reboot
    echo [OK] Device rebooting.
    goto end
)
if "%frp_again%"=="4" goto end
goto frp_loop

:all
call :hosts
call :bloatware
call :services
echo.
echo [OK] All ad-blocking measures applied.
goto menu_loop

:menu_loop
echo ============================================
echo   Done! What next?
echo ============================================
echo.
echo   1. Return to menu
echo   2. Reboot device
echo   3. Exit
echo.
set /p "again=  Enter choice [1-3]: "
if "%again%"=="1" goto main_menu
if "%again%"=="2" (
    echo [*] Rebooting device...
    adb reboot
    echo [OK] Device rebooting.
    goto end
)
if "%again%"=="3" goto end
goto menu_loop

:end
echo.
echo [*] If device is stuck, unplug USB and force reboot.
echo.
pause
