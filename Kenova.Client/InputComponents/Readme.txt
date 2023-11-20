Signed
        SByte,  /* sbyte */
        Int16,  /* short */
        Int32,  /* int */
        Int64,  /* long */
Unsigned
        Byte,   /* byte */
        UInt16, /* ushort */
        UInt32, /* uint */
        UInt64, /* ulong */
Floating point
		Single, /* float */
        Double, /* double */


Use DECIMAL for money values! Not floats and doubles.
https://stackoverflow.com/questions/3730019/why-not-use-double-or-float-to-represent-currency/3730040#3730040

Representing money as a double or float will probably look good at first as the software rounds off the tiny errors,
but as you perform more additions, subtractions, multiplications and divisions on inexact numbers, 
errors will compound and you'll end up with values that are visibly not accurate. This makes floats and doubles
inadequate for dealing with money, where perfect accuracy for multiples of base 10 powers is required.

When to use Decimal, Double or Float?
http://net-informations.com/q/faq/float.html

Float - 32 bit (7 digits) <-- Use a float  for speed in calculating coordinates
Double - 64 bit (15-16 digits) <-- XAML coordinate system
Decimal - 128 bit (28-29 significant digits) <-- Always use Decimal for money.

                                 Min/MaxValue
                     Implemented Attribute    Min/MaxValue
								 problem
InputString		     Yes   	 	 --			  --

InputDate		     Yes         --			  --              (implement preservation of timepart)
InputDateNullable    Yes		 --			  --

InputDecimal         Yes         Yes		  Yes

InputInt64           Yes         Yes		  Yes

InputTime			 No          --           --              (implement preservation of datepart)


Pending: InputStringNullable, InputInt64Nullable....

Convert XAML string to value:
http://timheuer.com/blog/archive/2017/02/15/implement-type-converter-uwp-winrt-windows-10-xaml.aspx
[Windows.Foundation.Metadata.CreateFromString(MethodName = "CustomControlWithType.Location.ConvertToLatLong")]

UWP for absolute beginners:
https://windowsdeveloper.azureedge.net/pdfs/Universal%20Windows%20Platform%20for%20Absolute%20Beginners.pdf