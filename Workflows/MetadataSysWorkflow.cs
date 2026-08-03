using System.Runtime.InteropServices;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// MetadataSys COM surface funnel: standard COM lifetime, class factory,
/// registration pair, and the WLXPSGetItemPropertyHandler stub which is
/// hard-coded to E_NOTIMPL with a NULL out-parameter.
/// Values pinned from the MetadataSys source.
/// </summary>
public static class MetadataSysWorkflow
{
    private const int S_OK = 0;
    private const int CLASS_E_CLASSNOTAVAILABLE = unchecked((int)0x80040111);
    private const uint E_NOTIMPL = 0x80004001;

    public static IEnumerable<TestCase> All()
    {
        yield return T.Test("MetadataSys.dll", "workflow", "mds:wf:lifetime",
            "COM lifetime: DllCanUnloadNow -> S_OK", ctx => DllApi.Mds_DllCanUnloadNow() == S_OK);

        yield return T.Test("MetadataSys.dll", "workflow", "mds:wf:class-factory",
            "class factory: DllGetClassObject -> CLASS_E_CLASSNOTAVAILABLE",
            ctx => DllApi.Mds_DllGetClassObject(IntPtr.Zero, IntPtr.Zero, out _) == CLASS_E_CLASSNOTAVAILABLE);

        yield return T.Test("MetadataSys.dll", "workflow", "mds:wf:registration",
            "registration pair: Register + Unregister -> S_OK", ctx =>
                DllApi.Mds_DllRegisterServer() == S_OK && DllApi.Mds_DllUnregisterServer() == S_OK);

        yield return T.Test("MetadataSys.dll", "workflow", "mds:wf:property-handler",
            "WLXPSGetItemPropertyHandler -> E_NOTIMPL, out NULL", ctx =>
            {
                IntPtr riid = Marshal.AllocHGlobal(16);
                try
                {
                    Marshal.WriteInt64(riid, 0, 0x0000000000000011);
                    Marshal.WriteInt64(riid, 8, 0);
                    IntPtr ppv = Marshal.AllocHGlobal(IntPtr.Size);
                    Marshal.WriteIntPtr(ppv, IntPtr.Zero);
                    try
                    {
                        int hr = DllApi.WLXPSGetItemPropertyHandler(IntPtr.Zero, 0, riid, out IntPtr result);
                        return (uint)hr == E_NOTIMPL && result == IntPtr.Zero;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ppv);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(riid);
                }
            });
    }
}
