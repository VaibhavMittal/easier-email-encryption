Imports System
Imports System.Globalization

Imports FirstOultookAddin.Org.BouncyCastle.Crypto
Imports FirstOultookAddin.Org.BouncyCastle.Crypto.Digests
Imports FirstOultookAddin.Org.BouncyCastle.Crypto.Prng

Namespace Org.BouncyCastle.Security
    Public Class SecureRandom
        Inherits Random
        ' Note: all objects of this class should be deriving their random data from
        ' a single generator appropriate to the digest being used.
        Private Shared ReadOnly sha1Generator As IRandomGenerator = New DigestRandomGenerator(New Sha1Digest())
        Private Shared ReadOnly sha256Generator As IRandomGenerator = New DigestRandomGenerator(New Sha256Digest())

        Private Shared ReadOnly m_master As SecureRandom() = {Nothing}
        Private Shared ReadOnly Property Master() As SecureRandom
            Get
                If m_master(0) Is Nothing Then
                    Dim gen As IRandomGenerator = sha256Generator
                    gen = New ReversedWindowGenerator(gen, 32)
                    Dim sr As SecureRandom = InlineAssignHelper(m_master(0), New SecureRandom(gen))

                    sr.SetSeed(DateTime.Now.Ticks)
                    sr.SetSeed(New ThreadedSeedGenerator().GenerateSeed(24, True))
                    sr.GenerateSeed(1 + sr.[Next](32))
                End If

                Return m_master(0)
            End Get
        End Property

        Public Shared Function GetInstance(algorithm As String) As SecureRandom
            ' TODO Compared to JDK, we don't auto-seed if the client forgets - problem?

            ' TODO Support all digests more generally, by stripping PRNG and calling DigestUtilities?
            Dim drg As IRandomGenerator = Nothing
            Select Case algorithm.ToUpper(CultureInfo.InvariantCulture)
                Case "SHA1PRNG"
                    drg = sha1Generator
                    Exit Select
                Case "SHA256PRNG"
                    drg = sha256Generator
                    Exit Select
            End Select

            If drg IsNot Nothing Then
                Return New SecureRandom(drg)
            End If

            Throw New ArgumentException("Unrecognised PRNG algorithm: " + algorithm, "algorithm")
        End Function

        Public Shared Function GetSeed(length As Integer) As Byte()
            Return Master.GenerateSeed(length)
        End Function

        Protected generator As IRandomGenerator

        Public Sub New()
            Me.New(sha1Generator)
            SetSeed(GetSeed(8))
        End Sub

        Public Sub New(inSeed As Byte())
            Me.New(sha1Generator)
            SetSeed(inSeed)
        End Sub

        ''' <summary>Use the specified instance of IRandomGenerator as random source.</summary>
        ''' <remarks>
        ''' This constructor performs no seeding of either the <c>IRandomGenerator</c> or the
        ''' constructed <c>SecureRandom</c>. It is the responsibility of the client to provide
        ''' proper seed material as necessary/appropriate for the given <c>IRandomGenerator</c>
        ''' implementation.
        ''' </remarks>
        ''' <param name="generator">The source to generate all random bytes from.</param>
        Public Sub New(generator As IRandomGenerator)
            MyBase.New(0)
            Me.generator = generator
        End Sub

        Public Overridable Function GenerateSeed(length As Integer) As Byte()
            SetSeed(DateTime.Now.Ticks)

            Dim rv As Byte() = New Byte(length - 1) {}
            NextBytes(rv)
            Return rv
        End Function

        Public Overridable Sub SetSeed(inSeed As Byte())
            generator.AddSeedMaterial(inSeed)
        End Sub

        Public Overridable Sub SetSeed(seed As Long)
            generator.AddSeedMaterial(seed)
        End Sub

        Public Overrides Function [Next]() As Integer
            While True
                Dim i As Integer = NextInt() And Integer.MaxValue

                If i <> Integer.MaxValue Then
                    Return i
                End If
            End While
        End Function

        Public Overrides Function [Next](maxValue As Integer) As Integer
            If maxValue < 2 Then
                If maxValue < 0 Then
                    Throw New ArgumentOutOfRangeException("maxValue < 0")
                End If

                Return 0
            End If

            ' Test whether maxValue is a power of 2
            If (maxValue And -maxValue) = maxValue Then
                Dim val As Integer = NextInt() And Integer.MaxValue
                Dim lr As Long = (CLng(maxValue) * CLng(val)) >> 31
                Return CInt(lr)
            End If

            Dim bits As Integer, result As Integer
            Do
                bits = NextInt() And Integer.MaxValue
                result = bits Mod maxValue
            Loop While bits - result + (maxValue - 1) < 0
            ' Ignore results near overflow
            Return result
        End Function

        Public Overrides Function [Next](minValue As Integer, maxValue As Integer) As Integer
            If maxValue <= minValue Then
                If maxValue = minValue Then
                    Return minValue
                End If

                Throw New ArgumentException("maxValue cannot be less than minValue")
            End If

            Dim diff As Integer = maxValue - minValue
            If diff > 0 Then
                Return minValue + [Next](diff)
            End If

            While True
                Dim i As Integer = NextInt()

                If i >= minValue AndAlso i < maxValue Then
                    Return i
                End If
            End While
        End Function

        Public Overrides Sub NextBytes(buffer As Byte())
            generator.NextBytes(buffer)
        End Sub

        Public Overridable Sub NextBytes(buffer As Byte(), start As Integer, length As Integer)
            generator.NextBytes(buffer, start, length)
        End Sub

        Private Shared ReadOnly DoubleScale As Double = System.Math.Pow(2.0, 64.0)

        Public Overrides Function NextDouble() As Double
            Return Convert.ToDouble(CULng(NextLong())) / DoubleScale
        End Function

        Public Overridable Function NextInt() As Integer
            Dim intBytes As Byte() = New Byte(3) {}
            NextBytes(intBytes)

            Dim result As Integer = 0
            For i As Integer = 0 To 3
                result = (result << 8) + (intBytes(i) And &Hff)
            Next

            Return result
        End Function

        Public Overridable Function NextLong() As Long
            Return (CLng(CUInt(NextInt())) << 32) Or CLng(CUInt(NextInt()))
        End Function
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace