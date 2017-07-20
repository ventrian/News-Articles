Namespace Ventrian.NewsArticles.Components.Common

    Public Class ArticleUtilities

        Public Shared Function MapPath(ByVal path As String)

            If (HttpContext.Current IsNot Nothing) Then
                Return HttpContext.Current.Server.MapPath(path)
            End If

            Return path

        End Function

        Public Shared Function ResolveUrl(ByVal path As String)

            Return SafeToAbsolute(path)

        End Function

        Public Shared Function ToAbsoluteUrl(relativeUrl As String) As String
            If String.IsNullOrEmpty(relativeUrl) Then
                Return relativeUrl
            End If

            If HttpContext.Current Is Nothing Then
                Return relativeUrl
            End If

            Dim url = HttpContext.Current.Request.Url
            Dim port = If(url.Port <> 80, (":" & url.Port), [String].Empty)

            If relativeUrl.StartsWith("~") Then
                Return [String].Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, SafeToAbsolute(relativeUrl))
            Else
                Return [String].Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, relativeUrl)
            End If
        End Function

        Private Shared Function SafeToAbsolute(path As String) As String
            Dim madeSafe As String = path.Replace("?", "UNLIKELY_TOKEN")
            Dim absolute As String = VirtualPathUtility.ToAbsolute(madeSafe)
            Dim restored As String = absolute.Replace("UNLIKELY_TOKEN", "?")
            Return restored
        End Function

    End Class

End Namespace
