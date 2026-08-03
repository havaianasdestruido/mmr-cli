using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// WLMFReadWrite null-guard funnel. Only functions that validate their
/// HANDLE argument without constructing a ReaderManager/WriterManager are
/// exercised (no Media Foundation side effects): the reader/writer accessors
/// return E_INVALIDARG on a NULL handle and Close is a safe no-op.
/// </summary>
public static class MfReadWriteWorkflow
{
    private const uint E_INVALIDARG = 0x80070057;

    public static IEnumerable<TestCase> All()
    {
        yield return T.Test("WLMFReadWrite.dll", "workflow", "mfrw:wf:reader-guards",
            "reader guards: GetProperties/ReadFrame(NULL) -> E_INVALIDARG", ctx =>
            {
                if ((uint)DllApi.MFReader_GetProperties(IntPtr.Zero, IntPtr.Zero) != E_INVALIDARG) return false;
                if ((uint)DllApi.MFReader_ReadFrame(IntPtr.Zero, 0, null, 0, out _) != E_INVALIDARG) return false;
                return true;
            });

        yield return T.Test("WLMFReadWrite.dll", "workflow", "mfrw:wf:writer-guards",
            "writer guards: WriteFrame/Finalize(NULL) -> E_INVALIDARG", ctx =>
            {
                if ((uint)DllApi.MFWriter_WriteFrame(IntPtr.Zero, null, 0, 0) != E_INVALIDARG) return false;
                if ((uint)DllApi.MFWriter_Finalize(IntPtr.Zero) != E_INVALIDARG) return false;
                return true;
            });

        yield return T.Test("WLMFReadWrite.dll", "workflow", "mfrw:wf:close-null",
            "MFReader_Close(NULL) is a safe no-op", _ => { DllApi.MFReader_Close(IntPtr.Zero); return true; });
    }
}
