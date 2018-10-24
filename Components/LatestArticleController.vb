Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Common.Utilities
Imports System.Xml

Namespace Ventrian.NewsArticles

    Public Class LatestArticleController
        Implements IPortable

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule

            Dim objModuleController As New ModuleController
            Dim settings As Hashtable = Common.GetModuleSettings(ModuleID)

            Dim objLatestLayoutController As New LatestLayoutController()

            Dim objLayoutHeader As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Header_Html, ModuleID, settings)
            Dim objLayoutItem As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Item_Html, ModuleID, settings)
            Dim objLayoutFooter As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Footer_Html, ModuleID, settings)
            Dim objLayoutEmpty As LayoutInfo = objLatestLayoutController.GetLayout(LatestLayoutType.Listing_Empty_Html, ModuleID, settings)

            Dim strXML As String = ""

            strXML += "<layoutHeader>" & XmlUtils.XMLEncode(objLayoutHeader.Template) & "</layoutHeader>"
            strXML += "<layoutItem>" & XmlUtils.XMLEncode(objLayoutItem.Template) & "</layoutItem>"
            strXML += "<layoutFooter>" & XmlUtils.XMLEncode(objLayoutFooter.Template) & "</layoutFooter>"
            strXML += "<layoutEmpty>" & XmlUtils.XMLEncode(objLayoutEmpty.Template) & "</layoutEmpty>"

            Return strXML

        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements IPortable.ImportModule

            Dim objXmlDocument As New XmlDocument()
            objXmlDocument.LoadXml("<xml>" & Content & "</xml>")

            Dim objLatestLayoutController As New LatestLayoutController()
            For Each xmlChildNode As XmlNode In objXmlDocument.ChildNodes(0).ChildNodes
                If (xmlChildNode.Name = "layoutHeader") Then
                    objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Header_Html, ModuleID, xmlChildNode.InnerText)
                End If
                If (xmlChildNode.Name = "layoutItem") Then
                    objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Item_Html, ModuleID, xmlChildNode.InnerText)
                End If
                If (xmlChildNode.Name = "layoutFooter") Then
                    objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Footer_Html, ModuleID, xmlChildNode.InnerText)
                End If
                If (xmlChildNode.Name = "layoutEmpty") Then
                    objLatestLayoutController.UpdateLayout(LatestLayoutType.Listing_Empty_Html, ModuleID, xmlChildNode.InnerText)
                End If
            Next

        End Sub

    End Class

End Namespace