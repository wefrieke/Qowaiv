﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qowaiv.UnitTests.Json;
using Qowaiv.UnitTests.TestTools;
using Qowaiv.UnitTests.TestTools.Formatting;
using Qowaiv.UnitTests.TestTools.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Qowaiv.UnitTests
{
    /// <summary>Tests the week date SVO.</summary>
    [TestClass]
    public class WeekDateTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly WeekDate TestStruct = new WeekDate(1997, 14, 6);

        #region week date const tests

        /// <summary>WeekDate.MinValue should be equal to the default of week date.</summary>
        [TestMethod]
        public void MinValue_None_EqualsDefault()
        {
            Assert.AreEqual(default(WeekDate), WeekDate.MinValue);
        }

        #endregion

        #region week date constructor tests

        [TestMethod]
        public void Ctor_Y0_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(0000, 10, 4);
            },
            "year",
            "Year should be in range [1,9999].");
        }
        [TestMethod]
        public void Ctor_Y10000_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(10000, 10, 4);
            },
            "year",
            "Year should be in range [1,9999].");
        }

        [TestMethod]
        public void Ctor_W0_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(1980, 0, 4);
            },
            "week",
            "Week should be in range [1,53].");
        }
        [TestMethod]
        public void Ctor_W54_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(1980, 54, 4);
            },
            "week",
            "Week should be in range [1,53].");
        }

        [TestMethod]
        public void Ctor_D0_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(1980, 10, 0);
            },
            "day", 
            "Day should be in range [1,7].");
        }
        [TestMethod]
        public void Ctor_D8_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(1980, 10, 8);
            },
            "day",
            "Day should be in range [1,7].");
        }

        [TestMethod]
        public void Ctor_Y9999W52D6_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(9999, 52, 6);
            },
            null,
            "Year, Week, and Day parameters describe an un-representable Date.");
        }

        [TestMethod]
        public void Ctor_Y9999W53D1_ThrowsArgumentOutofRangeException()
        {
            ExceptionAssert.ExpectArgumentOutOfRangeException(() =>
            {
                new WeekDate(9999, 53, 6);
            },
            null,
            "Year, Week, and Day parameters describe an un-representable Date.");
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse null should be valid.</summary>
        [TestMethod]
        public void TyrParse_Null_IsInvalid()
        {
            WeekDate val;

            string str = null;
            Assert.IsFalse(WeekDate.TryParse(str, out val), "Not valid");
        }

        /// <summary>TryParse string.Empty should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringEmpty_IsInvalid()
        {
            WeekDate val;

            string str = string.Empty;

            Assert.IsFalse(WeekDate.TryParse(str, out val), "Not valid");
        }

        /// <summary>TryParse with specified string value should be valid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsValid()
        {
            WeekDate val;

            string str = "1234-W50-6";

            Assert.IsTrue(WeekDate.TryParse(str, out val), "Valid");
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        /// <summary>TryParse with specified string value should be invalid.</summary>
        [TestMethod]
        public void TyrParse_StringValue_IsNotValid()
        {
            WeekDate val;

            string str = "string";

            Assert.IsFalse(WeekDate.TryParse(str, out val), "Valid");
        }

        [TestMethod]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (new CultureInfoScope("en-GB"))
            {
                ExceptionAssert.ExpectException<FormatException>
                (() =>
                {
                    WeekDate.Parse("InvalidInput");
                },
                "Not a valid week date");
            }
        }

        [TestMethod]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = TestStruct;
                var act = WeekDate.TryParse(exp.ToString());

                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void TryParse_InvalidInput_DefaultValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                var exp = default(WeekDate);
                var act = WeekDate.TryParse("InvalidInput");

                Assert.AreEqual(exp, act);
            }
        }

        [TestMethod]
        public void TryParse_Y0000W21D7_DefaultValue()
        {
            WeekDate exp = default(WeekDate);
            WeekDate act;
            Assert.IsFalse(WeekDate.TryParse("0000-W21-7", out act));
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void TryParse_Y2000W53D7_DefaultValue()
        {
            WeekDate exp = default(WeekDate);
            WeekDate act;
            Assert.IsFalse(WeekDate.TryParse("2000-W53-7", out act));
            Assert.AreEqual(exp, act);
        }

        #endregion

        #region (XML) (De)serialization tests

        [TestMethod]
        public void Constructor_SerializationInfoIsNull_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException
            (() =>
            {
                SerializationTest.DeserializeUsingConstructor<WeekDate>(null, default(StreamingContext));
            },
            "info");
        }

        [TestMethod]
        public void Constructor_InvalidSerializationInfo_ThrowsSerializationException()
        {
            ExceptionAssert.ExpectException<SerializationException>
            (() =>
            {
                var info = new SerializationInfo(typeof(WeekDate), new System.Runtime.Serialization.FormatterConverter());
                SerializationTest.DeserializeUsingConstructor<WeekDate>(info, default(StreamingContext));
            });
        }

        [TestMethod]
        public void GetObjectData_Null_ThrowsArgumentNullException()
        {
            ExceptionAssert.ExpectArgumentNullException
            (() =>
            {
                ISerializable obj = TestStruct;
                obj.GetObjectData(null, default(StreamingContext));
            },
            "info");
        }

        [TestMethod]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(WeekDate), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default(StreamingContext));

            Assert.AreEqual((DateTime)TestStruct, info.GetDateTime("Value"));
        }

        [TestMethod]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = WeekDateTest.TestStruct;
            var exp = WeekDateTest.TestStruct;
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = WeekDateTest.TestStruct;
            var exp = WeekDateTest.TestStruct;
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void XmlSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = WeekDateTest.TestStruct;
            var exp = WeekDateTest.TestStruct;
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void SerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void DataContractSerializeDeserialize_WeekDateSerializeObject_AreEqual()
        {
            var input = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.DataContractSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [TestMethod]
        public void SerializeDeserialize_Empty_AreEqual()
        {
            var input = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDateTest.TestStruct,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.SerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [TestMethod]
        public void XmlSerializeDeserialize_Empty_AreEqual()
        {
            var input = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var exp = new WeekDateSerializeObject()
            {
                Id = 17,
                Obj = WeekDate.MinValue,
                Date = new DateTime(1970, 02, 14),
            };
            var act = SerializationTest.XmlSerializeDeserialize(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

        [TestMethod]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            Assert.IsNull(obj.GetSchema());
        }

        #endregion

        #region JSON (De)serialization tests

        [TestMethod]
        public void FromJson_Null_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<WeekDate>();
            },
            "JSON deserialization from null is not supported.");
        }

        [TestMethod]
        public void FromJson_InvalidStringValue_AssertFormatException()
        {
            ExceptionAssert.ExpectException<FormatException>(() =>
            {
                JsonTester.Read<WeekDate>("InvalidStringValue");
            },
            "Not a valid week date");
        }
        [TestMethod]
        public void FromJson_StringValue_AreEqual()
        {
            var act = JsonTester.Read<WeekDate>(TestStruct.ToString(CultureInfo.InvariantCulture));
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void FromJson_Int64Value_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<WeekDate>(123456L);
            },
            "JSON deserialization from an integer is not supported.");
        }

        [TestMethod]
        public void FromJson_DoubleValue_AssertNotSupportedException()
        {
            ExceptionAssert.ExpectException<NotSupportedException>(() =>
            {
                JsonTester.Read<WeekDate>(1234.56);
            },
            "JSON deserialization from a number is not supported.");
        }

        [TestMethod]
        public void FromJson_DateTimeValue_AreEqual()
        {
            var act = JsonTester.Read<WeekDate>((DateTime)TestStruct);
            var exp = TestStruct;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToJson_DefaultValue_AreEqual()
        {
            object act = JsonTester.Write(default(WeekDate));
            object exp = "0001-W01-1";

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void ToJson_TestStruct_AreEqual()
        {
            var act = JsonTester.Write(TestStruct);
            var exp = TestStruct.ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(exp, act);
        }

        #endregion

        #region IFormattable / ToString tests

        [TestMethod]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("Unit Test Format", new UnitTestFormatProvider());
            var exp = "Unit Test Formatter, value: '1997-W14-6', format: 'Unit Test Format'";

            Assert.AreEqual(exp, act);
        }
        
        [TestMethod]
        public void ToString_TestStruct_ComplexPattern()
        {
            var act = TestStruct.ToString("");
            var exp = "1997-W14-6";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_Y1979W3D5FormatW_ComplexPattern()
        {
            var act = new WeekDate(1979, 3, 5).ToString(@"y-\WW-d");
            var exp = "1979-W3-5";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void ToString_Y1979W3D5Formatw_ComplexPattern()
        {
            var act = new WeekDate(1979, 3, 5).ToString(@"y-\Ww-d");
            var exp = "1979-W03-5";
            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void DebugToString_TestStruct_String()
        {
            DebuggerDisplayAssert.HasResult("1997-W14-6", TestStruct);
        }

        #endregion

        #region IEquatable tests

        /// <summary>GetHash should not fail for WeekDate.Empty.</summary>
        [TestMethod]
        public void GetHash_Empty_Hash()
        {
            Assert.AreEqual(0, WeekDate.MinValue.GetHashCode());
        }

        /// <summary>GetHash should not fail for the test struct.</summary>
        [TestMethod]
        public void GetHash_TestStruct_Hash()
        {
            Assert.AreEqual(2027589483, WeekDateTest.TestStruct.GetHashCode());
        }

        [TestMethod]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = WeekDate.Parse("1997-14-6", CultureInfo.InvariantCulture);
            var r = WeekDate.Parse("1997-W14-6", CultureInfo.InvariantCulture);

            Assert.IsTrue(l.Equals(r));
        }

        [TestMethod]
        public void Equals_TestStructTestStruct_IsTrue()
        {
            Assert.IsTrue(WeekDateTest.TestStruct.Equals(WeekDateTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructEmpty_IsFalse()
        {
            Assert.IsFalse(WeekDateTest.TestStruct.Equals(WeekDate.MinValue));
        }

        [TestMethod]
        public void Equals_EmptyTestStruct_IsFalse()
        {
            Assert.IsFalse(WeekDate.MinValue.Equals(WeekDateTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructObjectTestStruct_IsTrue()
        {
            Assert.IsTrue(WeekDateTest.TestStruct.Equals((object)WeekDateTest.TestStruct));
        }

        [TestMethod]
        public void Equals_TestStructNull_IsFalse()
        {
            Assert.IsFalse(WeekDateTest.TestStruct.Equals(null));
        }

        [TestMethod]
        public void Equals_TestStructObject_IsFalse()
        {
            Assert.IsFalse(WeekDateTest.TestStruct.Equals(new object()));
        }

        [TestMethod]
        public void OperatorIs_TestStructTestStruct_IsTrue()
        {
            var l = WeekDateTest.TestStruct;
            var r = WeekDateTest.TestStruct;
            Assert.IsTrue(l == r);
        }

        [TestMethod]
        public void OperatorIsNot_TestStructTestStruct_IsFalse()
        {
            var l = WeekDateTest.TestStruct;
            var r = WeekDateTest.TestStruct;
            Assert.IsFalse(l != r);
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of week dates ascending.</summary>
        [TestMethod]
        public void OrderBy_WeekDate_AreEqual()
        {
            var item0 = WeekDate.Parse("2000-W01-3");
            var item1 = WeekDate.Parse("2000-W11-2");
            var item2 = WeekDate.Parse("2000-W21-1");
            var item3 = WeekDate.Parse("2000-W31-7");

            var inp = new List<WeekDate>() { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
            var exp = new List<WeekDate>() { WeekDate.MinValue, WeekDate.MinValue, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Orders a list of week dates descending.</summary>
        [TestMethod]
        public void OrderByDescending_WeekDate_AreEqual()
        {
            var item0 = WeekDate.Parse("2000-W01-3");
            var item1 = WeekDate.Parse("2000-W11-2");
            var item2 = WeekDate.Parse("2000-W21-1");
            var item3 = WeekDate.Parse("2000-W31-7");

            var inp = new List<WeekDate>() { WeekDate.MinValue, item3, item2, item0, item1, WeekDate.MinValue };
            var exp = new List<WeekDate>() { item3, item2, item1, item0, WeekDate.MinValue, WeekDate.MinValue };
            var act = inp.OrderByDescending(item => item).ToList();

            CollectionAssert.AreEqual(exp, act);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [TestMethod]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = (object)TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            Assert.AreEqual(exp, act);
        }

        /// <summary>Compare with null should throw an expception.</summary>
        [TestMethod]
        public void CompareTo_null_ThrowsArgumentException()
        {
            ExceptionAssert.ExpectArgumentException
            (() =>
                {
                    object other = null;
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a week date"
            );
        }
        /// <summary>Compare with a random object should throw an expception.</summary>
        [TestMethod]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            ExceptionAssert.ExpectArgumentException
            (() =>
                {
                    object other = new object();
                    var act = TestStruct.CompareTo(other);
                },
                "obj",
                "Argument must be a week date"
            );
        }

        [TestMethod]
        public void LessThan_17LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l < r);
        }
        [TestMethod]
        public void GreaterThan_21LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l > r);
        }

        [TestMethod]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l <= r);
        }
        [TestMethod]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 19, 5);

            Assert.IsTrue(l >= r);
        }

        [TestMethod]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 17, 5);
            WeekDate r = new WeekDate(1980, 17, 5);

            Assert.IsTrue(l <= r);
        }
        [TestMethod]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            WeekDate l = new WeekDate(1980, 21, 5);
            WeekDate r = new WeekDate(1980, 21, 5);

            Assert.IsTrue(l >= r);
        }
        #endregion

        #region Casting tests

        [TestMethod]
        public void Explicit_StringToWeekDate_AreEqual()
        {
            var exp = TestStruct;
            var act = (WeekDate)TestStruct.ToString();

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_WeekDateToString_AreEqual()
        {
            var exp = TestStruct.ToString();
            var act = (string)TestStruct;

            Assert.AreEqual(exp, act);
        }


        [TestMethod]
        public void Explicit_Int32ToWeekDate_AreEqual()
        {
            var exp = TestStruct;
            var act = (WeekDate)new DateTime(1997, 04, 05);

            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Explicit_WeekDateToInt32_AreEqual()
        {
            DateTime exp = new DateTime(1997, 04, 05);
            DateTime act = TestStruct;

            Assert.AreEqual(exp, act);
        }
        #endregion

        #region Properties

        [TestMethod]
        public void Date_TestStruct_AreEqual()
        {
            var exp = new DateTime(1997, 04, 05);
            var act = TestStruct.Date;

            Assert.AreEqual(exp, act);
        }


        [TestMethod]
        public void Year_MinValue_AreEqual()
        {
            var exp = WeekDate.MaxValue.Year;
            var act = 9999;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Year_MaxValue_AreEqual()
        {
            var exp = WeekDate.MaxValue.Year;
            var act = 9999;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Year_Y2010W52D7_AreEqual()
        {
            var date = new WeekDate(2010, 52, 7);
            var exp = 2010;
            var act = date.Year;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Year_Y2020W01D1_AreEqual()
        {
            var date = new WeekDate(2020, 01, 1);
            var exp = 2020;
            var act = date.Year;

            Assert.AreEqual(exp, act);
        }




        [TestMethod]
        public void Day_TestStruct_AreEqual()
        {
            var exp = 6;
            var act = TestStruct.Day;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void Day_Sunday_AreEqual()
        {
            var date = new WeekDate(1990, 40, 7);
            var exp = 7;
            var act = date.Day;

            Assert.AreEqual(exp, act);
        }

        [TestMethod]
        public void DayOfYear_TestStruct_AreEqual()
        {
            var exp = 96;
            var act = TestStruct.DayOfYear;

            Assert.AreEqual(exp, act);
        }



        #endregion

        #region Type converter tests

        [TestMethod]
        public void ConverterExists_WeekDate_IsTrue()
        {
            TypeConverterAssert.ConverterExists(typeof(WeekDate));
        }

        [TestMethod]
        public void CanNotConvertFromInt32_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanNotConvertFrom(typeof(WeekDate), typeof(Int32));
        }
        [TestMethod]
        public void CanNotConvertToInt32_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanNotConvertTo(typeof(WeekDate), typeof(Int32));
        }

        [TestMethod]
        public void CanConvertFromString_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanConvertFromString(typeof(WeekDate));
        }

        [TestMethod]
        public void CanConvertToString_WeekDate_IsTrue()
        {
            TypeConverterAssert.CanConvertToString(typeof(WeekDate));
        }

        [TestMethod]
        public void ConvertFromString_StringValue_TestStruct()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertFromEquals(WeekDateTest.TestStruct, WeekDateTest.TestStruct.ToString());
            }
        }

        [TestMethod]
        public void ConvertToString_TestStruct_StringValue()
        {
            using (new CultureInfoScope("en-GB"))
            {
                TypeConverterAssert.ConvertToStringEquals(WeekDateTest.TestStruct.ToString(), WeekDateTest.TestStruct);
            }
        }

        #endregion

        #region IsValid tests

        [TestMethod]
        public void IsValid_Data_IsFalse()
        {
            Assert.IsFalse(WeekDate.IsValid("Complex"), "Complex");
            Assert.IsFalse(WeekDate.IsValid((String)null), "(String)null");
            Assert.IsFalse(WeekDate.IsValid(String.Empty), "String.Empty");
            Assert.IsFalse(WeekDate.IsValid("0000-W12-6"), "0000-W12-6");
            Assert.IsFalse(WeekDate.IsValid("0001-W12-8"), "0001-W12-8");
            Assert.IsFalse(WeekDate.IsValid("9999-W53-1"), "9999-W53-1");
        }
        [TestMethod]
        public void IsValid_Data_IsTrue()
        {
            Assert.IsTrue(WeekDate.IsValid("1234-50-6"));
        }
        #endregion
    }

    [Serializable]
    public class WeekDateSerializeObject
    {
        public int Id { get; set; }
        public WeekDate Obj { get; set; }
        public DateTime Date { get; set; }
    }
}