Imports DotNetNuke.Instrumentation

Module LogginUtils
    Public Function Logger() As ILog
        return LoggerSource.Instance.GetLogger("Ventrian.NA")
    End Function
End Module
