Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Net

Namespace Ventrian.NewsArticles

    Public Class bitly
        Private loginAccount As String
        Private apiKeyForAccount As String

        Private submitPath As String = "http://api.bit.ly/shorten?version=2.0.1&format=xml"
        Private errorStatus As Integer = 0
        Private errorStatusMessage As String = ""


        ' Constructors (overloaded and chained)
        Public Sub New()
            Me.New("bitlyapidemo", "R_0da49e0a9118ff35f52f629d2d71bf07")
        End Sub


        Public Sub New(ByVal login As String, ByVal APIKey As String)
            loginAccount = login
            apiKeyForAccount = APIKey

            submitPath &= "&login=" & loginAccount & "&apiKey=" & apiKeyForAccount
        End Sub


        ' Properties to retrieve error information.
        Public ReadOnly Property ErrorCode() As Integer
            Get
                Return errorStatus
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return errorStatusMessage
            End Get
        End Property


        ' Main shorten function which takes in the long URL and returns the bit.ly shortened URL
        Public Function Shorten(ByVal url As String) As String

            errorStatus = 0
            errorStatusMessage = ""

            Dim doc As XmlDocument
            doc = buildDocument(url)

            If Not doc.DocumentElement Is Nothing Then

                Dim shortenedNode As XmlNode = doc.DocumentElement.SelectSingleNode("results/nodeKeyVal/shortUrl")

                If Not shortenedNode Is Nothing Then

                    Return shortenedNode.InnerText

                Else

                    getErrorCode(doc)

                End If
            Else

                errorStatus = -1
                errorStatusMessage = "Unable to connect to bit.ly for shortening of URL"
            End If

            Return ""

        End Function


        ' Sets error code and message in the situation we receive a response, but there was
        ' something wrong with our submission.
        Private Sub getErrorCode(ByVal doc As XmlDocument)

            Dim errorNode As XmlNode = doc.DocumentElement.SelectSingleNode("errorCode")
            Dim errorMessageNode As XmlNode = doc.DocumentElement.SelectSingleNode("errorMessage")

            If Not errorNode Is Nothing Then

                errorStatus = Convert.ToInt32(errorNode.InnerText)
                errorStatusMessage = errorMessageNode.InnerText
            End If
        End Sub


        ' Builds an XmlDocument using the XML returned by bit.ly in response
        ' to our URL being submitted
        Private Function buildDocument(ByVal url As String) As XmlDocument

            Dim doc As New XmlDocument

            Try

                ' Load the XML response into an XML Document and return it.
                doc.LoadXml(readSource(submitPath + "&longUrl=" + url))
                Return doc

            Catch e As Exception

                Return New XmlDocument()
            End Try
        End Function


        ' Fetches a result from bit.ly provided the URL submitted
        Private Function readSource(ByVal url As String) As String
            Dim client As New WebClient

            Try

                Using reader As New StreamReader(client.OpenRead(url))
                    ' Read all of the response
                    Return reader.ReadToEnd()
                    reader.Close()
                End Using

            Catch e As Exception
                Throw e
            End Try

        End Function

    End Class

End Namespace
