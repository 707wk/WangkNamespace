Public Module ModuleDES
#Region "DES"
    ''' <summary>
    ''' DES加密
    ''' </summary>
    <Obsolete>
    Public Function EncryptDES(ByVal SourceStr As String,
                               ByVal myKey As String,
                               Optional ByVal myIV As String = "00000000") As String
        Return DESHelper.EncryptDES(SourceStr, myKey, myIV)
    End Function

    ''' <summary>
    ''' DES解密
    ''' </summary>
    <Obsolete>
    Public Function DecryptDES(ByVal SourceStr As String,
                               ByVal myKey As String,
                               Optional ByVal myIV As String = "00000000") As String
        Return DESHelper.DecryptDES(SourceStr, myKey, myIV)
    End Function
#End Region

#Region "TripleDES"
    ''' <summary>
    ''' TripleDES加密
    ''' </summary>
    <Obsolete>
    Public Function EncryptTripleDES(ByVal SourceStr As String,
                                     Optional ByVal myKey As String = "",
                                     Optional ByVal myIV As String = "00000000000000000000000000000000") As String
        Return DESHelper.EncryptTripleDES(SourceStr, myKey, myIV)
    End Function

    ''' <summary>
    ''' TripleDES解密
    ''' </summary>
    <Obsolete>
    Public Function DecryptTripleDES(ByVal SourceStr As String,
                                     Optional ByVal myKey As String = "",
                                     Optional ByVal myIV As String = "00000000000000000000000000000000") As String
        Return DESHelper.DecryptTripleDES(SourceStr, myKey, myIV)
    End Function
#End Region

End Module
