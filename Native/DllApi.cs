using System.Runtime.InteropServices;
using System.Text;

namespace MmrCli.Native;

/// <summary>
/// P/Invoke surface for every callable export in the 21-DLL set.
/// __stdcall is the default on x86; __cdecl is declared explicitly.
/// WLXPhotoSqm / DmxBici exports are only reachable under their mangled
/// names (the .def aliases are not resolvable via GetProcAddress), so the
/// mangled name is used as EntryPoint.
/// </summary>
public static class DllApi
{
    // ================= wlidcli.dll (__stdcall) =================
    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int WLGetEnvironment(out IntPtr env);

    [DllImport("wlidcli.dll", SetLastError = true)]
    public static extern uint WLCreateIdentityHandle();

    [DllImport("wlidcli.dll", SetLastError = true)]
    public static extern int WLIsSignedIn(uint handle);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int WLClogin(IntPtr hwndParent, string? cred, uint flags, out IntPtr authState);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int WLCheckCredentials(string? cred);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int WLGetTicket(uint handle, out IntPtr ticket);

    [DllImport("wlidcli.dll", SetLastError = true)]
    public static extern void WLFreeMemory(IntPtr pv);

    // ================= uxctl.dll (__stdcall) =================
    [DllImport("uxctl.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int UxControlsInitProcess();

    [DllImport("uxctl.dll", SetLastError = true)]
    public static extern void UxControlsUninitProcess();

    [DllImport("uxctl.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int UxControlsCreateObject(out IntPtr ppObject);

    // ================= WLXVideoTrim.dll (__stdcall) =================
    [DllImport("WLXVideoTrim.dll")]
    public static extern int CreateAVICopierDirect(out IntPtr ppUnknown);

    [DllImport("WLXVideoTrim.dll")]
    public static extern int CreateVideoPlayer(out IntPtr ppUnknown);

    [DllImport("WLXVideoTrim.dll")]
    public static extern int CreateVideoFormatContextTranscoder(out IntPtr ppUnknown);

    [DllImport("WLXVideoTrim.dll")]
    public static extern int CreateVideoWMVTranscoder(out IntPtr ppUnknown);

    [DllImport("WLXVideoTrim.dll")]
    public static extern int CreateVideoCopierFromMediaType(IntPtr pMediaType, out IntPtr ppUnknown);

    // ================= WLXPipetran.dll (__stdcall) =================
    [DllImport("WLXPipetran.dll")]
    public static extern int GetTFXCreateFunctions(out IntPtr ppFunctions, out uint pCount);

    // ================= MetadataSys.dll (__stdcall) =================
    [DllImport("MetadataSys.dll")]
    public static extern int WLXPSGetItemPropertyHandler(IntPtr pItem, uint dwAccessMode, IntPtr riid, out IntPtr ppv);

    // ================= WLMFReadWrite.dll (__stdcall, decorated names) =================
    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFReader_Open@4", CharSet = CharSet.Unicode)]
    public static extern IntPtr MFReader_Open(string? path);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFReader_Close@4")]
    public static extern void MFReader_Close(IntPtr hReader);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFReader_GetProperties@8")]
    public static extern int MFReader_GetProperties(IntPtr hReader, IntPtr pProps);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFReader_ReadFrame@24")]
    public static extern int MFReader_ReadFrame(IntPtr hReader, long llTimeMs, byte[]? buffer, uint cbBuffer, out uint cbRead);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFWriter_Create@8", CharSet = CharSet.Unicode)]
    public static extern IntPtr MFWriter_Create(string? path, IntPtr pProps);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFWriter_WriteFrame@20")]
    public static extern int MFWriter_WriteFrame(IntPtr hWriter, byte[]? data, uint cbData, long llTimeMs);

    [DllImport("WLMFReadWrite.dll", EntryPoint = "_MFWriter_Finalize@4")]
    public static extern int MFWriter_Finalize(IntPtr hWriter);

    // ================= WLXMediaPublishSubscribe.dll =================
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PublishConfig
    {
        public int Target;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)] public string Title;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)] public string Description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)] public string Tags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Category;
        public int Private;
        public int AllowEmbed;
        public uint PrivacyLevel;
    }

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_Create@0")]
    public static extern IntPtr PublishManager_Create();

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_Destroy@4")]
    public static extern void PublishManager_Destroy(IntPtr hManager);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_EnumerateTargets@12")]
    public static extern int PublishManager_EnumerateTargets(IntPtr hManager, [In, Out] int[] targets, ref uint count);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetTargetName@12", CharSet = CharSet.Unicode)]
    public static extern int PublishManager_GetTargetName(int target, StringBuilder name, uint cch);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_Authenticate@12")]
    public static extern int PublishManager_Authenticate(IntPtr hManager, int target, IntPtr hwndParent);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_IsAuthenticated@12")]
    public static extern int PublishManager_IsAuthenticated(IntPtr hManager, int target, out int pAuth);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_SignOut@8")]
    public static extern int PublishManager_SignOut(IntPtr hManager, int target);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_StartPublish@16", CharSet = CharSet.Unicode)]
    public static extern IntPtr PublishManager_StartPublish(IntPtr hManager, int target, string filePath, ref PublishConfig config);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetStatus@12")]
    public static extern int PublishManager_GetStatus(IntPtr hPublish, out uint status, out uint percent);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_Cancel@4")]
    public static extern int PublishManager_Cancel(IntPtr hPublish);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetResult@8")]
    public static extern int PublishManager_GetResult(IntPtr hPublish, IntPtr pResult);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_SetProgressCallback@12")]
    public static extern int PublishManager_SetProgressCallback(IntPtr hPublish, IntPtr pfn, IntPtr userData);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_SetCompleteCallback@12")]
    public static extern int PublishManager_SetCompleteCallback(IntPtr hPublish, IntPtr pfn, IntPtr userData);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_StartSubscribe@12", CharSet = CharSet.Unicode)]
    public static extern IntPtr PublishManager_StartSubscribe(IntPtr hManager, int target, string itemId);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetSubscribeStatus@12")]
    public static extern int PublishManager_GetSubscribeStatus(IntPtr hSubscribe, out uint status, out uint percent);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetAccountInfo@16", CharSet = CharSet.Unicode)]
    public static extern int PublishManager_GetAccountInfo(IntPtr hManager, int target, StringBuilder name, uint cch);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_RefreshToken@8")]
    public static extern int PublishManager_RefreshToken(IntPtr hManager, int target);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_SetDefaultTarget@8")]
    public static extern int PublishManager_SetDefaultTarget(IntPtr hManager, int target);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetDefaultTarget@8")]
    public static extern int PublishManager_GetDefaultTarget(IntPtr hManager, out int target);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_GetServiceStatus@8")]
    public static extern int PublishManager_GetServiceStatus(int target, out int available);

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "_PublishManager_Cleanup@0")]
    public static extern int PublishManager_Cleanup();

    // ================= WLXMP4Parser.dll =================
    [DllImport("WLXMP4Parser.dll", EntryPoint = "_AddMP4SourceFilter@12", CharSet = CharSet.Unicode)]
    public static extern int AddMP4SourceFilter(string? path, IntPtr pGraph, out IntPtr ppFilter);

    [DllImport("WLXMP4Parser.dll", EntryPoint = "_BuildMP4FilterGraph@8", CharSet = CharSet.Unicode)]
    public static extern int BuildMP4FilterGraph(string? path, out IntPtr ppGraph);

    [DllImport("WLXMP4Parser.dll", EntryPoint = "_BuildMP4PlayBack@8", CharSet = CharSet.Unicode)]
    public static extern int BuildMP4PlayBack(string? path, IntPtr pGraph);

    [DllImport("WLXMP4Parser.dll", EntryPoint = "_IsMP4FilePlayable@4", CharSet = CharSet.Unicode)]
    public static extern int IsMP4FilePlayable(string? path);

    // ================= WLXPipeline.dll =================
    [DllImport("WLXPipeline.dll", EntryPoint = "_GetPipelineCreateFunctions@8")]
    public static extern int GetPipelineCreateFunctions(out IntPtr ppFunctions, out uint pCount);

    // ================= WLXPhotoBase.dll =================
    [DllImport("WLXPhotoBase.dll", EntryPoint = "_WLXPhotoBase_Init@0")]
    public static extern void WLXPhotoBase_Init();

    // ================= MovieMakerCore.dll (__cdecl) =================
    [DllImport("MovieMakerCore.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int MovieMakerMain(int argc, string[]? argv);

    // ================= WLXPhotoSqm.dll (mangled entry points) =================
    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Startup@Sqm@@YGXXZ")]
    public static extern void Sqm_Startup();

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Startup@Sqm@@YGXW4SqmDmxAppId@1@@Z")]
    public static extern void Sqm_StartupWithAppId(uint appId);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Shutdown@Sqm@@YGXXZ")]
    public static extern void Sqm_Shutdown();

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?GetOptInState@Sqm@@YG?AW4OptInState@1@XZ")]
    public static extern uint Sqm_GetOptInState();

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?IsEnabled@Sqm@@YG_NXZ")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Sqm_IsEnabled();

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Set@Sqm@@YGXKK@Z")]
    public static extern void Sqm_Set(uint id, uint value);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Set@Sqm@@YGXKPB_W@Z", CharSet = CharSet.Unicode)]
    public static extern void Sqm_SetString(uint id, string? value);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?SetIfMax@Sqm@@YGXKK@Z")]
    public static extern void Sqm_SetIfMax(uint id, uint value);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?SetIfMin@Sqm@@YGXKK@Z")]
    public static extern void Sqm_SetIfMin(uint id, uint value);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?Increment@Sqm@@YGXKK@Z")]
    public static extern void Sqm_Increment(uint id, uint increment);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?AddToAverage@Sqm@@YGXKK@Z")]
    public static extern void Sqm_AddToAverage(uint id, uint value);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?StartTimer@Sqm@@YGXK@Z")]
    public static extern void Sqm_StartTimer(uint id);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?PauseTimer@Sqm@@YGXK@Z")]
    public static extern void Sqm_PauseTimer(uint id);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?StartStreamTimer@Sqm@@YGXKKK@Z")]
    public static extern void Sqm_StartStreamTimer(uint a, uint b, uint c);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?StopStreamTimer@Sqm@@YGXKK@Z")]
    public static extern void Sqm_StopStreamTimer(uint a, uint b);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?AbortStreamTimer@Sqm@@YGXKK@Z")]
    public static extern void Sqm_AbortStreamTimer(uint a, uint b);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?ReportAppLaunchStatus@Sqm@@YGX_N@Z")]
    public static extern void Sqm_ReportAppLaunchStatus([MarshalAs(UnmanagedType.I1)] bool success);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?ReportAppCloseStatus@Sqm@@YGX_N@Z")]
    public static extern void Sqm_ReportAppCloseStatus([MarshalAs(UnmanagedType.I1)] bool success);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?IsStreamTimerActive@Sqm@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Sqm_IsStreamTimerActive(uint a, uint b);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?IsStreamTimerDataSet@Sqm@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Sqm_IsStreamTimerDataSet(uint a, uint b);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?SetAppStatusReportingMode@Sqm@@YGX_N@Z")]
    public static extern void Sqm_SetAppStatusReportingMode([MarshalAs(UnmanagedType.I1)] bool mode);

    [DllImport("WLXPhotoSqm.dll", EntryPoint = "?SetOptInPreference@Sqm@@YGX_N@Z")]
    public static extern void Sqm_SetOptInPreference([MarshalAs(UnmanagedType.I1)] bool optIn);

    // ================= DmxBici.dll (mangled entry points) =================
    [DllImport("DmxBici.dll", EntryPoint = "?StartExperience@BiciWrapper@@YGJXZ")]
    public static extern int Bici_StartExperience();

    [DllImport("DmxBici.dll", EntryPoint = "?StartExperience@BiciWrapper@@YGJW4BiciStartupId@1@@Z")]
    public static extern int Bici_StartExperienceWithId(uint id);

    [DllImport("DmxBici.dll", EntryPoint = "?EndExperience@BiciWrapper@@YGJXZ")]
    public static extern int Bici_EndExperience();

    [DllImport("DmxBici.dll", EntryPoint = "?SetAnid@BiciWrapper@@YGJPB_W@Z", CharSet = CharSet.Unicode)]
    public static extern int Bici_SetAnid(string? anid);

    [DllImport("DmxBici.dll", EntryPoint = "?Set@BiciWrapper@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_Set(uint key, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?Increment@BiciWrapper@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_Increment(uint key, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?SetIfMax@BiciWrapper@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_SetIfMax(uint key, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?SetIfMin@BiciWrapper@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_SetIfMin(uint key, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?AddToAverage@BiciWrapper@@YG_NKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_AddToAverage(uint key, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?SetString@BiciWrapper@@YG_NKPB_W@Z", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_SetString(uint key, string? value);

    [DllImport("DmxBici.dll", EntryPoint = "?AddToDataPoint@BiciWrapper@@YG_NKKK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_AddToDataPoint(uint key, uint subKey, uint value);

    [DllImport("DmxBici.dll", EntryPoint = "?AddStringToDataPoint@BiciWrapper@@YG_NKKPB_W@Z", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_AddStringToDataPoint(uint key, uint subKey, string? value);

    [DllImport("DmxBici.dll", EntryPoint = "?TimerStart@BiciWrapper@@YG_NK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TimerStart(uint timerId);

    [DllImport("DmxBici.dll", EntryPoint = "?TimerAccumulate@BiciWrapper@@YG_NK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TimerAccumulate(uint timerId);

    [DllImport("DmxBici.dll", EntryPoint = "?TimerRecord@BiciWrapper@@YG_NK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TimerRecord(uint timerId);

    [DllImport("DmxBici.dll", EntryPoint = "?TransferExperienceToAppId@BiciWrapper@@YG_NK@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TransferExperienceToAppId(uint appId);

    [DllImport("DmxBici.dll", EntryPoint = "?TransferExperienceToApp@BiciWrapper@@YG_NPAPA_W@Z")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TransferExperienceToApp(out IntPtr names);

    [DllImport("DmxBici.dll", EntryPoint = "?TransferExperienceToWeb@BiciWrapper@@YG_NPB_WPAPA_W@Z", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Bici_TransferExperienceToWeb(string? url, out IntPtr parameters);

    [DllImport("DmxBici.dll", EntryPoint = "?AddToStream@BiciWrapper@@YGXKPBVTuple@1@@Z")]
    public static extern void Bici_AddToStream(uint key, IntPtr tuple);

    // ================= UXCore.dll (mixed calling conventions) =================
    // __stdcall (decorated names)
    [DllImport("UXCore.dll", EntryPoint = "_UXCoreInitProcess@0")]
    public static extern int UXCoreInitProcess();

    [DllImport("UXCore.dll", EntryPoint = "_UXCoreInitThread@0")]
    public static extern int UXCoreInitThread();

    [DllImport("UXCore.dll", EntryPoint = "_UXCoreUnInitProcess@0")]
    public static extern void UXCoreUnInitProcess();

    [DllImport("UXCore.dll", EntryPoint = "_UXCoreUnInitThread@0")]
    public static extern void UXCoreUnInitThread();

    [DllImport("UXCore.dll", EntryPoint = "_UxGetClassObject@12")]
    public static extern int UxGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);

    [DllImport("UXCore.dll", EntryPoint = "_DuiCreateObject@8", CharSet = CharSet.Unicode)]
    public static extern int DuiCreateObject(string? className, out IntPtr ppElement);

    // __cdecl (plain names)
    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr RMFindModule(IntPtr hMod, [MarshalAs(UnmanagedType.LPWStr)] string? name);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr RMFindModuleForResource(IntPtr hMod, uint id);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void RMUpdateResourceSet(IntPtr hMod);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public static extern int RMLoadString(IntPtr hinst, uint id, StringBuilder buf, int cch);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr RMLoadStringBSTR(IntPtr hinst, uint id);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr RMLoadImage(IntPtr hinst, uint id);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr RMLoadMenu(IntPtr hinst, uint id);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int StrToID([MarshalAs(UnmanagedType.LPWStr)] string? str);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int PeekMessageEx(IntPtr msg, IntPtr hWnd, uint min, uint max, uint removeMsg, uint flags);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int LayerManagerInitThread();

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void LayerManagerUnInitThread();

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr DuiGetLayerManager();

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ElementFromGadget(IntPtr hGadget);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetGadgetRect(IntPtr hGadget, IntPtr rc);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetGadgetSize(IntPtr hGadget, IntPtr sz);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetTopHWNDParent(IntPtr hWnd);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr Internal_GetKeyFocusedElement_HWNDElement(IntPtr hwndElement);

    [DllImport("UXCore.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetKeyFocusedElement(IntPtr hwndElement);

    // ================= COM boilerplate (all __stdcall) =================
    [DllImport("MetadataSys.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Mds_DllCanUnloadNow();
    [DllImport("MetadataSys.dll", EntryPoint = "DllGetClassObject")] public static extern int Mds_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("MetadataSys.dll", EntryPoint = "DllRegisterServer")] public static extern int Mds_DllRegisterServer();
    [DllImport("MetadataSys.dll", EntryPoint = "DllUnregisterServer")] public static extern int Mds_DllUnregisterServer();

    [DllImport("WLMFDS.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Mfds_DllCanUnloadNow();
    [DllImport("WLMFDS.dll", EntryPoint = "DllGetClassObject")] public static extern int Mfds_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLMFDS.dll", EntryPoint = "DllRegisterServer")] public static extern int Mfds_DllRegisterServer();
    [DllImport("WLMFDS.dll", EntryPoint = "DllUnregisterServer")] public static extern int Mfds_DllUnregisterServer();

    [DllImport("MovieMakerPreviewClient.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Preview_DllCanUnloadNow();
    [DllImport("MovieMakerPreviewClient.dll", EntryPoint = "DllGetClassObject")] public static extern int Preview_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("MovieMakerPreviewClient.dll", EntryPoint = "DllRegisterServer")] public static extern int Preview_DllRegisterServer();
    [DllImport("MovieMakerPreviewClient.dll", EntryPoint = "DllUnregisterServer")] public static extern int Preview_DllUnregisterServer();

    [DllImport("WLXPhotoCinematic.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Cinematic_DllCanUnloadNow();
    [DllImport("WLXPhotoCinematic.dll", EntryPoint = "DllGetClassObject")] public static extern int Cinematic_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXPhotoCinematic.dll", EntryPoint = "DllRegisterServer")] public static extern int Cinematic_DllRegisterServer();
    [DllImport("WLXPhotoCinematic.dll", EntryPoint = "DllUnregisterServer")] public static extern int Cinematic_DllUnregisterServer();

    [DllImport("WLXMovieLibrary.dll", EntryPoint = "DllCanUnloadNow")] public static extern int MovieLibrary_DllCanUnloadNow();
    [DllImport("WLXMovieLibrary.dll", EntryPoint = "DllGetClassObject")] public static extern int MovieLibrary_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXMovieLibrary.dll", EntryPoint = "DllRegisterServer")] public static extern int MovieLibrary_DllRegisterServer();
    [DllImport("WLXMovieLibrary.dll", EntryPoint = "DllUnregisterServer")] public static extern int MovieLibrary_DllUnregisterServer();

    [DllImport("WLXSlideshow.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Slideshow_DllCanUnloadNow();
    [DllImport("WLXSlideshow.dll", EntryPoint = "DllGetClassObject")] public static extern int Slideshow_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXSlideshow.dll", EntryPoint = "DllRegisterServer")] public static extern int Slideshow_DllRegisterServer();
    [DllImport("WLXSlideshow.dll", EntryPoint = "DllUnregisterServer")] public static extern int Slideshow_DllUnregisterServer();

    [DllImport("WLXFaceRecognition.dll", EntryPoint = "DllCanUnloadNow")] public static extern int FaceRecog_DllCanUnloadNow();
    [DllImport("WLXFaceRecognition.dll", EntryPoint = "DllGetClassObject")] public static extern int FaceRecog_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXFaceRecognition.dll", EntryPoint = "DllRegisterServer")] public static extern int FaceRecog_DllRegisterServer();
    [DllImport("WLXFaceRecognition.dll", EntryPoint = "DllUnregisterServer")] public static extern int FaceRecog_DllUnregisterServer();

    [DllImport("WLXMP4Parser.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Mp4_DllCanUnloadNow();
    [DllImport("WLXMP4Parser.dll", EntryPoint = "DllGetClassObject")] public static extern int Mp4_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXMP4Parser.dll", EntryPoint = "DllRegisterServer")] public static extern int Mp4_DllRegisterServer();
    [DllImport("WLXMP4Parser.dll", EntryPoint = "DllUnregisterServer")] public static extern int Mp4_DllUnregisterServer();

    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Mps_DllCanUnloadNow();
    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "DllGetClassObject")] public static extern int Mps_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);
    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "DllRegisterServer")] public static extern int Mps_DllRegisterServer();
    [DllImport("WLXMediaPublishSubscribe.dll", EntryPoint = "DllUnregisterServer")] public static extern int Mps_DllUnregisterServer();

    [DllImport("WLXPipeline.dll", EntryPoint = "DllRegisterServer")] public static extern int Pipeline_DllRegisterServer();

    [DllImport("WLMFReadWrite.dll", EntryPoint = "DllCanUnloadNow")] public static extern int Mfrw_DllCanUnloadNow();
    [DllImport("WLMFReadWrite.dll", EntryPoint = "DllGetClassObject")] public static extern int Mfrw_DllGetClassObject(IntPtr rclsid, IntPtr riid, out IntPtr ppv);

    // ================= Ole32 =================
    [DllImport("ole32.dll")]
    public static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

    [DllImport("ole32.dll")]
    public static extern void CoUninitialize();

    public const uint COINIT_APARTMENTTHREADED = 0x2;
}
