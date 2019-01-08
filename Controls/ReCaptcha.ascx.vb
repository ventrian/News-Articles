Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.NewsArticles.Controls
    Public Class ReCaptcha
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            reCaptchaDiv.Controls.Clear()
            reCaptchaDiv.Controls.Add(New LiteralControl($"<div class=""g-recaptcha"" data-sitekey=""{GetSiteKey}"" style=""display: inline-block;""></div>"))
        End Sub

        public property SiteKey as String = ""
        public property SecretKey as String = ""

        Protected Sub RecaptchaValidator_OnServerValidate(source As Object, args As ServerValidateEventArgs)
            args.IsValid = RecaptchaIsValid()
        End Sub

        Protected Function GetSiteKey() As String
            return SiteKey
        End Function

        Private _recaptchaisvalid As Boolean? = Nothing
        Public Function RecaptchaIsValid() As Boolean
            If _recaptchaisvalid.HasValue Then Return _recaptchaisvalid.Value
            Dim Response As String = Request("g-recaptcha-response")
            RecaptchaIsValid = False
            Dim req As HttpWebRequest = CType(WebRequest.Create($"https://www.google.com/recaptcha/api/siteverify"), HttpWebRequest)

            Try
                Dim postData = $"secret={SecretKey}&response={Response}"
                Dim postEnc = Encoding.ASCII.GetBytes(postData)
                req.Method = "POST"
                req.ContentType = "application/x-www-form-urlencoded"
                req.ContentLength = postEnc.Length

                Using stream = req.GetRequestStream()
                    stream.Write(postEnc, 0, postEnc.Length)
                End Using

                Using wResponse As WebResponse = req.GetResponse()

                    Using readStream As StreamReader = New StreamReader(wResponse.GetResponseStream())
                        Dim jsonResponse As String = readStream.ReadToEnd()
                        Dim js As JavaScriptSerializer = New JavaScriptSerializer()
                        Dim data As RecaptchaResponse = js.Deserialize(Of RecaptchaResponse)(jsonResponse)
                        _recaptchaisvalid = Convert.ToBoolean(data.success)
                    End Using
                End Using

            Catch ex As WebException
                Throw ex
            End Try

            Return _recaptchaisvalid.Value
        End Function
    End Class
    public class RecaptchaResponse
        public Property success As String
    End Class

End Namespace