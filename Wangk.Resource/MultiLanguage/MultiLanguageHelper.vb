''' <summary>
''' 多语言辅助类
''' </summary>
Public NotInheritable Class MultiLanguageHelper
    '单例模式

    Private Sub New()
    End Sub

    ''' <summary>
    ''' 实例
    ''' </summary>
    Private Shared instance As MultiLanguage

    ''' <summary>
    ''' 参数
    ''' </summary>
    Public Shared ReadOnly Property Lang As MultiLanguage
        Get
            Return instance
        End Get
    End Property

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <param name="Lang">显示语言</param>
    ''' <param name="AppName">应用名</param>
    Public Shared Sub Init(Lang As Wangk.Resource.MultiLanguage.LANG,
                           AppName As String)

        instance = New Wangk.Resource.MultiLanguage
        instance.Init(Lang, AppName)

    End Sub
End Class
