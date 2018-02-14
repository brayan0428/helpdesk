Public Class Variables

    Public Shared pxServido As String = "192.168.100.5"
    Private Shared pxUsuario As String = "sa_ofimatica"
    Private Shared pxContras As String = "Pa$$w0rd"

    'Public Shared pxServido As String = "10.10.10.64"
    'Private Shared pxUsuario As String = "sa"
    'Private Shared pxContras As String = "Pa$$w0rd"

    'Conexion Ofima
    Private Shared pxBaseDat As String = "Cajacopi"
    Private Shared pxBaseCtr As String = "CONTROL_OFIMAEnterprise" ' "CONTROL_OFIMA2015_01"

    'Conexiones
    Public Shared Conexion_Dato As String = "Data Source=" & pxServido.Trim & ";Initial Catalog=" & pxBaseDat.Trim & ";User ID=" & pxUsuario.Trim & ";Password=" & pxContras.Trim & ";"
    Public Shared Conexion_User As String = "Data Source=" & pxServido.Trim & ";Initial Catalog=" & pxBaseCtr.Trim & ";User ID=" & pxUsuario.Trim & ";Password=" & pxContras.Trim & ";"

    Public Shared tFooter As String = DateTime.Now.Year.ToString & " ® Cajacopi. Coordinación TIC"
End Class
