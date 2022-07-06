﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace wsStoryFeed
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="StoryFeedWSSoap", [Namespace]:="http://smart-thinker.com/webservices")>  _
    Partial Public Class StoryFeedWS
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private AddActionOperationCompleted As System.Threading.SendOrPostCallback
        
        Private AddActionIfNotExistsOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.My.MySettings.Default.Ventrian_NewsArticles_wsStoryFeed_StoryFeedWS
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event AddActionCompleted As AddActionCompletedEventHandler
        
        '''<remarks/>
        Public Event AddActionIfNotExistsCompleted As AddActionIfNotExistsCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smart-thinker.com/webservices/AddAction", RequestNamespace:="http://smart-thinker.com/webservices", ResponseNamespace:="http://smart-thinker.com/webservices", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function AddAction(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal key As String) As Integer
            Dim results() As Object = Me.Invoke("AddAction", New Object() {actionType, relatedID, actionText, createdByUserID, key})
            Return CType(results(0),Integer)
        End Function
        
        '''<remarks/>
        Public Overloads Sub AddActionAsync(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal key As String)
            Me.AddActionAsync(actionType, relatedID, actionText, createdByUserID, key, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub AddActionAsync(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal key As String, ByVal userState As Object)
            If (Me.AddActionOperationCompleted Is Nothing) Then
                Me.AddActionOperationCompleted = AddressOf Me.OnAddActionOperationCompleted
            End If
            Me.InvokeAsync("AddAction", New Object() {actionType, relatedID, actionText, createdByUserID, key}, Me.AddActionOperationCompleted, userState)
        End Sub
        
        Private Sub OnAddActionOperationCompleted(ByVal arg As Object)
            If (Not (Me.AddActionCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent AddActionCompleted(Me, New AddActionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://smart-thinker.com/webservices/AddActionIfNotExists", RequestNamespace:="http://smart-thinker.com/webservices", ResponseNamespace:="http://smart-thinker.com/webservices", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function AddActionIfNotExists(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal addIfNotExistsOnly As Boolean, ByVal key As String) As Integer
            Dim results() As Object = Me.Invoke("AddActionIfNotExists", New Object() {actionType, relatedID, actionText, createdByUserID, addIfNotExistsOnly, key})
            Return CType(results(0),Integer)
        End Function
        
        '''<remarks/>
        Public Overloads Sub AddActionIfNotExistsAsync(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal addIfNotExistsOnly As Boolean, ByVal key As String)
            Me.AddActionIfNotExistsAsync(actionType, relatedID, actionText, createdByUserID, addIfNotExistsOnly, key, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub AddActionIfNotExistsAsync(ByVal actionType As Integer, ByVal relatedID As Integer, ByVal actionText As String, ByVal createdByUserID As Integer, ByVal addIfNotExistsOnly As Boolean, ByVal key As String, ByVal userState As Object)
            If (Me.AddActionIfNotExistsOperationCompleted Is Nothing) Then
                Me.AddActionIfNotExistsOperationCompleted = AddressOf Me.OnAddActionIfNotExistsOperationCompleted
            End If
            Me.InvokeAsync("AddActionIfNotExists", New Object() {actionType, relatedID, actionText, createdByUserID, addIfNotExistsOnly, key}, Me.AddActionIfNotExistsOperationCompleted, userState)
        End Sub
        
        Private Sub OnAddActionIfNotExistsOperationCompleted(ByVal arg As Object)
            If (Not (Me.AddActionIfNotExistsCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent AddActionIfNotExistsCompleted(Me, New AddActionIfNotExistsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")>  _
    Public Delegate Sub AddActionCompletedEventHandler(ByVal sender As Object, ByVal e As AddActionCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class AddActionCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Integer
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Integer)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")>  _
    Public Delegate Sub AddActionIfNotExistsCompletedEventHandler(ByVal sender As Object, ByVal e As AddActionIfNotExistsCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class AddActionIfNotExistsCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Integer
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Integer)
            End Get
        End Property
    End Class
End Namespace
