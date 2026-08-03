namespace MmrCli.Native;

/// <summary>
/// Expected export surfaces per DLL, taken from dumpbin /exports verification
/// of the WMMR build_clean output (see the session export audit).
/// Presence probes use these exact names via GetProcAddress.
/// </summary>
public static class ExportCatalog
{
    public static readonly IReadOnlyDictionary<string, string[]> Catalogs = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        ["wlidcli.dll"] =
        [
            "WLClogin", "WLCheckCredentials", "WLCreateIdentityHandle",
            "WLGetTicket", "WLIsSignedIn", "WLFreeMemory", "WLGetEnvironment",
        ],

        ["uxctl.dll"] =
        [
            "UxControlsInitProcess", "UxControlsCreateObject", "UxControlsUninitProcess",
        ],

        ["WLXVideoTrim.dll"] =
        [
            "CreateAVICopierDirect", "CreateVideoCopierFromMediaType",
            "CreateVideoFormatContextTranscoder", "CreateVideoPlayer", "CreateVideoWMVTranscoder",
        ],

        ["WLXPipetran.dll"] =
        [
            "GetTFXCreateFunctions",
        ],

        ["MetadataSys.dll"] =
        [
            "WLXPSGetItemPropertyHandler",
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["MovieMakerPreviewClient.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLMFDS.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLXPhotoCinematic.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLXMovieLibrary.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLXSlideshow.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLXFaceRecognition.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
        ],

        ["WLMFReadWrite.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject",
            "_MFReader_Close@4", "_MFReader_GetProperties@8", "_MFReader_Open@4",
            "_MFReader_ReadFrame@24", "_MFWriter_Create@8", "_MFWriter_Finalize@4",
            "_MFWriter_WriteFrame@20",
        ],

        ["WLXMediaPublishSubscribe.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
            "_PublishManager_Authenticate@12", "_PublishManager_Cancel@4", "_PublishManager_Cleanup@0",
            "_PublishManager_Create@0", "_PublishManager_Destroy@4", "_PublishManager_EnumerateTargets@12",
            "_PublishManager_GetAccountInfo@16", "_PublishManager_GetDefaultTarget@8", "_PublishManager_GetResult@8",
            "_PublishManager_GetServiceStatus@8", "_PublishManager_GetStatus@12", "_PublishManager_GetSubscribeStatus@12",
            "_PublishManager_GetTargetName@12", "_PublishManager_IsAuthenticated@12", "_PublishManager_RefreshToken@8",
            "_PublishManager_SetCompleteCallback@12", "_PublishManager_SetDefaultTarget@8",
            "_PublishManager_SetProgressCallback@12", "_PublishManager_SignOut@8",
            "_PublishManager_StartPublish@16", "_PublishManager_StartSubscribe@12",
        ],

        ["WLXMP4Parser.dll"] =
        [
            "DllCanUnloadNow", "DllGetClassObject", "DllRegisterServer", "DllUnregisterServer",
            "_AddMP4SourceFilter@12", "_BuildMP4FilterGraph@8", "_BuildMP4PlayBack@8", "_IsMP4FilePlayable@4",
        ],

        ["WLXPipeline.dll"] =
        [
            "DllRegisterServer", "_GetPipelineCreateFunctions@8",
        ],

        ["WLXPhotoBase.dll"] =
        [
            "??0Exception@Base@@QAE@ABV01@@Z",
            "??0Exception@Base@@QAE@J@Z",
            "??0File@Base@@QAE@ABV01@@Z",
            "??0File@Base@@QAE@XZ",
            "??0FindFile@Base@@QAE@ABV01@@Z",
            "??0FindFile@Base@@QAE@XZ",
            "??0GdiException@Base@@QAE@ABV01@@Z",
            "??0GdiException@Base@@QAE@W4Status@Gdiplus@@@Z",
            "??0IntSet@Base@@QAE@ABV01@@Z",
            "??0IntSet@Base@@QAE@XZ",
            "??0TempFile@Base@@QAE@ABV01@@Z",
            "??0TempFile@Base@@QAE@XZ",
            "??0Thread@Base@@QAE@ABV01@@Z",
            "??0Thread@Base@@QAE@XZ",
            "??1Exception@Base@@UAE@XZ",
            "??1File@Base@@QAE@XZ",
            "??1FindFile@Base@@QAE@XZ",
            "??1GdiException@Base@@UAE@XZ",
            "??1IntSet@Base@@QAE@XZ",
            "??1TempFile@Base@@QAE@XZ",
            "??1Thread@Base@@UAE@XZ",
            "??2Exception@Base@@CAPAXI@Z",
            "??3Exception@Base@@CAXPAX@Z",
            "??4Exception@Base@@QAEAAV01@ABV01@@Z",
            "??4File@Base@@QAEAAV01@ABV01@@Z",
            "??4FindFile@Base@@QAEAAV01@ABV01@@Z",
            "??4GdiException@Base@@QAEAAV01@ABV01@@Z",
            "??4IntSet@Base@@QAEAAV01@ABV01@@Z",
            "??4TempFile@Base@@QAEAAV01@ABV01@@Z",
            "??4Thread@Base@@QAEAAV01@ABV01@@Z",
            "??BException@Base@@QBEJXZ",
            "??_7Exception@Base@@6B@",
            "??_7GdiException@Base@@6B@",
            "??_7Thread@Base@@6B@",
            "?Add@IntSet@Base@@QAEXH@Z",
            "?Clear@IntSet@Base@@QAEXXZ",
            "?Close@File@Base@@QAEXXZ",
            "?Close@FindFile@Base@@QAEXXZ",
            "?Close@TempFile@Base@@QAEXXZ",
            "?Contains@IntSet@Base@@QBE_NH@Z",
            "?Create@TempFile@Base@@QAE_NPB_W@Z",
            "?Delete@Private@Base@@YAXPAX@Z",
            "?FindFirstFileW@FindFile@Base@@QAE_NPB_W@Z",
            "?FindNextFileW@FindFile@Base@@QAE_NXZ",
            "?GdiplusStatusToHresult@Base@@YAJW4Status@Gdiplus@@@Z",
            "?GetAt@IntSet@Base@@QBEHI@Z",
            "?GetBaseStringManager@String@Base@@YAPAUIAtlStringMgr@ATL@@XZ",
            "?GetCount@IntSet@Base@@QBEIXZ",
            "?GetDirectory@FindFile@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetFile@TempFile@Base@@QAEAAVFile@2@XZ",
            "?GetFile@TempFile@Base@@QBEABVFile@2@XZ",
            "?GetFileName@File@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetFileName@FindFile@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetFilePath@FindFile@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetFileSize@File@Base@@QBE_KXZ",
            "?GetFileSize@FindFile@Base@@QBE_KXZ",
            "?GetFindData@FindFile@Base@@QBEABU_WIN32_FIND_DATAW@@XZ",
            "?GetFullPath@File@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetGdiplusStatus@GdiException@Base@@QBE?AW4Status@Gdiplus@@XZ",
            "?GetHResult@Exception@Base@@QBEJXZ",
            "?GetHandle@File@Base@@QBEPAXXZ",
            "?GetHandle@Thread@Base@@QBEPAXXZ",
            "?GetPath@File@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetPath@TempFile@Base@@QBE?AV?$CStringT@_WV?$StrTraitATL@_WV?$ChTraitsCRT@_W@ATL@@@ATL@@@ATL@@XZ",
            "?GetProcessorCount@CPU@Base@@YAHXZ",
            "?GetThreadId@Thread@Base@@QBEKXZ",
            "?IsDirectory@FindFile@Base@@QBE_NXZ",
            "?IsEmpty@IntSet@Base@@QBE_NXZ",
            "?IsFile@FindFile@Base@@QBE_NXZ",
            "?IsRunning@Thread@Base@@QBE_NXZ",
            "?IsValid@File@Base@@QBE_NXZ",
            "?IsValid@TempFile@Base@@QBE_NXZ",
            "?IsWin7OrGreater@OS@Base@@YA_NXZ",
            "?IsWin8OrGreater@OS@Base@@YA_NXZ",
            "?New@Private@Base@@YAPAXI_N@Z",
            "?Open@File@Base@@QAE_NPB_WKKK@Z",
            "?Read@File@Base@@QAEKPAXK@Z",
            "?Remove@IntSet@Base@@QAEXH@Z",
            "?Seek@File@Base@@QAE_K_JK@Z",
            "?SetHResult@Exception@Base@@QAEXJ@Z",
            "?ShouldStop@Thread@Base@@MBE_NXZ",
            "?Start@Thread@Base@@QAEJXZ",
            "?Stop@Thread@Base@@QAEXK@Z",
            "?ThreadProcThunk@Thread@Base@@CGKPAX@Z",
            "?Throw@Base@@YAXJ@Z",
            "?Throw@Exception@Base@@QBEXXZ",
            "?Throw@GdiException@Base@@UBEXXZ",
            "?ThrowLastError@Base@@YAXXZ",
            "?Wait@Thread@Base@@QAEJK@Z",
            "?Write@File@Base@@QAEKPBXK@Z",
            "_WLXPhotoBase_Init@0",
        ],

        ["WLXPhotoSqm.dll"] =
        [
            "?AbortStreamTimer@Sqm@@YGXKK@Z", "?AddStreamTimerData@Sqm@@YGXKKK@Z",
            "?AddStreamTimerData@Sqm@@YGXKKPB_W@Z", "?AddToAverage@Sqm@@YGXKK@Z",
            "?AddToStream@Sqm@@YGXKK@Z", "?AddToStream@Sqm@@YGXKKK@Z",
            "?AddToStream@Sqm@@YGXKKKK@Z", "?AddToStream@Sqm@@YGXKPB_W@Z",
            "?AddToStream@Sqm@@YGXKPBVTuple@1@@Z", "?AddToStreamTimer@Sqm@@YGXKKKPBVTuple@1@@Z",
            "?AddToStreamTimer@Sqm@@YGXKKPBVTuple@1@@Z", "?DeferAddToAverage@Sqm@@YGXKK@Z",
            "?DeferAddToMedian@Sqm@@YGXKK@Z", "?DeferReportAverage@Sqm@@YGXK@Z",
            "?DeferReportMax@Sqm@@YGXK@Z", "?DeferReportMedian@Sqm@@YGXK@Z",
            "?DeferReportMin@Sqm@@YGXK@Z", "?DeferSetIfMax@Sqm@@YGXKK@Z",
            "?DeferSetIfMin@Sqm@@YGXKK@Z", "?EnableShipAsserts@Sqm@@YGXXZ",
            "?GetOptInState@Sqm@@YG?AW4OptInState@1@XZ", "?Increment@Sqm@@YGXKK@Z",
            "?InitializeUserExecutedActionReporting@Sqm@@YGXKK@Z", "?IsEnabled@Sqm@@YG_NXZ",
            "?IsStreamTimerActive@Sqm@@YG_NKK@Z", "?IsStreamTimerDataSet@Sqm@@YG_NKK@Z",
            "?PauseTimer@Sqm@@YGXK@Z", "?ReportAppCloseStatus@Sqm@@YGX_N@Z",
            "?ReportAppLaunchStatus@Sqm@@YGX_N@Z", "?ReportUserExecutedAction@Sqm@@YGXKK@Z",
            "?Set@Sqm@@YGXKK@Z", "?Set@Sqm@@YGXKPB_W@Z",
            "?SetAppDefinedValue@Sqm@@YGXK@Z", "?SetAppStatusReportingMode@Sqm@@YGX_N@Z",
            "?SetApplicationMode@Sqm@@YGXK@Z", "?SetIfMax@Sqm@@YGXKK@Z",
            "?SetIfMin@Sqm@@YGXKK@Z", "?SetOptInPreference@Sqm@@YGX_N@Z",
            "?Shutdown@Sqm@@YGXXZ", "?StartStreamTimer@Sqm@@YGXKKK@Z",
            "?StartTimer@Sqm@@YGXK@Z", "?Startup@Sqm@@YGXW4SqmDmxAppId@1@@Z",
            "?Startup@Sqm@@YGXXZ", "?StopStreamTimer@Sqm@@YGXKK@Z",
        ],

        ["UXCore.dll"] =
        [
            "DuiGetLayerManager", "ElementFromGadget", "GetGadgetRect", "GetGadgetSize",
            "GetKeyFocusedElement", "GetMessageEx", "GetTopHWNDParent",
            "Internal_GetKeyFocusedElement_HWNDElement", "LayerManagerInitThread",
            "LayerManagerUnInitThread", "PeekMessageEx", "RMFindModule",
            "RMFindModuleForResource", "RMLoadImage", "RMLoadMenu", "RMLoadString",
            "RMLoadStringBSTR", "RMUpdateResourceSet", "StrToID",
            "_DuiCreateObject@8", "_UXCoreInitProcess@0", "_UXCoreInitThread@0",
            "_UXCoreUnInitProcess@0", "_UXCoreUnInitThread@0", "_UxGetClassObject@12",
        ],

        ["DmxBici.dll"] =
        [
            "?AddStringToDataPoint@BiciWrapper@@YG_NKKPB_W@Z", "?AddToAverage@BiciWrapper@@YG_NKK@Z",
            "?AddToDataPoint@BiciWrapper@@YG_NKKK@Z", "?AddToStream@BiciWrapper@@YGXKPBVTuple@1@@Z",
            "?EndExperience@BiciWrapper@@YGJXZ", "?Increment@BiciWrapper@@YG_NKK@Z",
            "?Set@BiciWrapper@@YG_NKK@Z", "?SetAnid@BiciWrapper@@YGJPB_W@Z",
            "?SetIfMax@BiciWrapper@@YG_NKK@Z", "?SetIfMin@BiciWrapper@@YG_NKK@Z",
            "?SetString@BiciWrapper@@YG_NKPB_W@Z", "?StartExperience@BiciWrapper@@YGJW4BiciStartupId@1@@Z",
            "?StartExperience@BiciWrapper@@YGJXZ", "?TimerAccumulate@BiciWrapper@@YG_NK@Z",
            "?TimerRecord@BiciWrapper@@YG_NK@Z", "?TimerStart@BiciWrapper@@YG_NK@Z",
            "?TransferExperienceToApp@BiciWrapper@@YG_NPAPA_W@Z", "?TransferExperienceToAppId@BiciWrapper@@YG_NK@Z",
            "?TransferExperienceToWeb@BiciWrapper@@YG_NPB_WPAPA_W@Z",
        ],

        ["GPURenderer.dll"] =
        [
            "??0GPURenderer@DirectUI@@QAE@XZ", "??1GPURenderer@DirectUI@@QAE@XZ",
            "??4GPURenderer@DirectUI@@QAEAAV01@ABV01@@Z", "?BeginDraw@GPURenderer@DirectUI@@QAEJXZ",
            "?Cleanup@GPURenderer@DirectUI@@AAEXXZ",
            "?DrawVideoFrame@GPURenderer@DirectUI@@QAEJPAUID3D11Texture2D@@ABUtagRECT@@@Z",
            "?EndDraw@GPURenderer@DirectUI@@QAEJXZ", "?GetD2DContext@GPURenderer@DirectUI@@QBEPAUID2D1DeviceContext@@XZ",
            "?GetDevice@GPURenderer@DirectUI@@QBEPAUID3D11Device@@XZ", "?Initialize@GPURenderer@DirectUI@@QAEJPAUHWND__@@@Z",
            "?Present@GPURenderer@DirectUI@@QAEJXZ", "?Resize@GPURenderer@DirectUI@@QAEJHH@Z",
        ],

        ["MovieMakerCore.dll"] =
        [
            "MovieMakerMain",
            "?Start@AVSource@HMRAVSource@@UAEJXZ",
            "?Stop@AVSource@HMRAVSource@@UAEJXZ",
            "?StoryboardManagerInitialize@StoryboardManager@@YAJXZ",
            "?StoryboardManagerIsInitialized@StoryboardManager@@YA_NXZ",
            "?Shutdown@AVSink@HMRAVSource@@QAEJXZ",
            "?Undo@MovieProject@StoryboardManager@@QAEJXZ",
            "?Shutdown@AVSourceProxy@HMRAVSource@@QAEJXZ",
            "?Validate@MovieProject@StoryboardManager@@QBEJXZ",
        ],
    };
}
