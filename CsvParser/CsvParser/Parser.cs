using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

using Parser.Converter;

namespace Parser
{
    namespace CSV
    {
        class CsvParser
        {
            private List<CsvRow> Rows;
            public char Delimita
            {
                get;
                private set;
            }
            public uint RowCount
            {
                get
                {
                    if (Rows == null)
                        return 0;
                    return (uint)Rows.Count;
                }
            }
            public uint MaxColumn
            {
                get;
                private set;
            }
            public uint MinColumn
            {
                get;
                private set;
            }
            public CsvParser()
            {
                Delimita = ',';
                MaxColumn = 0;
                MinColumn = uint.MaxValue;
            }

            public void parse(string data)
            {
                TextReader tr = new StreamReader(data);
                parse(tr);

            }

            public void parse(StreamReader sr)
            {
                TextReader tr = sr;
                parse(tr);
            }

            public int getInt(uint row, uint column)
            {
                if (Rows.Count <= row)
                    return 0;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return 0;
                return c.getInt();
            }

            public long getLong(uint row, uint column)
            {
                if (Rows.Count <= row)
                    return 0;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return 0;
                return c.getLong();
            }

            public string getString(uint row, uint column)
            {
                if (Rows.Count <= row)
                    return null;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return null;
                return c.getString();
            }

            public float getFloat(uint row, uint column)
            {
                if (Rows.Count <= row)
                    return 0;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return 0;
                return c.getFloat();
            }

            public double getDouble(uint row, uint column)
            {
                if (Rows.Count <= row)
                    return 0;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return 0;
                return c.getDouble();
            }

            public void get(uint row, uint column, IStringConverter conv)
            {
                if (Rows.Count <= row)
                    return;
                CsvRow r = Rows[(int)row];
                CsvColumn c = r.getColumn(column);
                if (c == null)
                    return;
                c.get(conv);
            }

            /// <summary>
            /// 指定された行の列数を取得します
            /// </summary>
            /// <param name="row">取得したい行( 0～N )</param>
            /// <returns>指定された列がない場合-> -1 : それ以外 -> 列数</returns>
            public int getColumnCount(uint row)
            {
                if (Rows.Count <= row)
                    return -1;
                return (int)Rows[(int)row].ColumnCount;
            }

            public void setDelimita(char delimita)
            {
                Delimita = delimita;
            }


            private void parse(TextReader tr)
            {
                Rows = new List<CsvRow>();
                //一行読む
                string line = "";
                while (tr.Peek() > 0)
                {
                    while (true)
                    {
                        line = tr.ReadLine();
                        //ダブルクォーテーションの個数を調べる
                        //ユーザーが書いたダブルクォーテーションに気を付ける
                        int quatCount = line.Length - line.Replace("\\\"", "").Replace("\"", "").Length;
                        //ダブルクォーテーションの個数が奇数だったならばセル内改行が存在するので
                        //読み込んだ文字列を連結する
                        if (quatCount % 2 == 0)
                        {
                            //偶数なら連結せず次へ
                            break;
                        }
                        if (tr.Peek() > 0)
                        {
                            throw new System.FormatException("このファイルはCSVファイルではない可能性があります。");
                        }
                    }
                    string[] col = line.Split(Delimita);
                    MaxColumn = (uint)Math.Max(MaxColumn, col.Length);
                    MinColumn = (uint)Math.Min(MinColumn, col.Length);
                    CsvRow row = new CsvRow(col);
                    Rows.Add(row);
                }
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int n = 0; n < Rows.Count; n++)
                {
                    sb.Append(Rows[n].ToString());
                    if (n < Rows.Count - 1)
                    {
                        sb.Append("\n");
                    }
                }
                return sb.ToString();
            }
        }

        class CsvRow
        {
            private CsvColumn[] Columns;
            public uint ColumnCount
            {
                get
                {
                    if (Columns == null)
                        return 0;
                    return (uint)Columns.Length;
                }
            }

            public CsvRow(string[] columns)
            {
                Columns = new CsvColumn[columns.Length];
                for (int n = 0; n < Columns.Length; n++)
                {
                    Columns[n] = new CsvColumn(columns[n]);
                }
            }

            public CsvColumn getColumn(uint column)
            {
                if (Columns.Length <= column)
                    return null;
                return Columns[column];
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                for (int n = 0; n < Columns.Length; n++)
                {
                    sb.Append(Columns[n].getString());
                    if (n < Columns.Length - 1)
                    {
                        sb.Append("_");
                    }
                }
                return sb.ToString();
            }
        }

        class CsvColumn
        {
            private string data;
            public CsvColumn(string str)
            {
                data = str;
            }

            public string getString()
            {
                return data;
            }

            public int getInt()
            {
                int result;
                if (int.TryParse(data, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }

            public long getLong()
            {
                long result;
                if (long.TryParse(data, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }

            public float getFloat()
            {
                float result;
                if (float.TryParse(data, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }

            public double getDouble()
            {
                double result;
                if (double.TryParse(data, out result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }

            public void get(IStringConverter conv)
            {
                conv.parse(data);
            }


            public override string ToString()
            {
                return data;
            }
        }
    }
    namespace Converter
    {
        interface IStringConverter
        {
            void parse(string data);
        }

        abstract class AStringConverter<T> : IStringConverter
        {
            protected T Data;

            public abstract T get();
            public abstract void parse(string data);
        }

        class IntegerConverter : AStringConverter<int>
        {
            public override void parse(string data)
            {
                int.TryParse(data, out Data);
            }
            public override int get()
            {
                return Data;
            }
        }

        class LongerConverter : AStringConverter<long>
        {
            public override void parse(string data)
            {
                long.TryParse(data, out Data);
            }
            public override long get()
            {
                return Data;
            }
        }

        class FloatConverter : AStringConverter<float>
        {
            public override void parse(string data)
            {
                float.TryParse(data, out Data);
            }
            public override float get()
            {
                return Data;
            }
        }

        class DoubleConverter : AStringConverter<double>
        {
            public override void parse(string data)
            {
                double.TryParse(data, out Data);
            }
            public override double get()
            {
                return Data;
            }
        }

        class CharsConverter : AStringConverter<char[]>
        {
            public override void parse(string data)
            {
                Data = data.ToCharArray();
            }
            public override char[] get()
            {
                return Data;
            }
        }

        class BytesConverter : AStringConverter<byte[]>
        { 
            private Encoding Enc;
            public BytesConverter(Encoding enc)
            {
                Enc = enc;
            }
            public override void parse(string data)
            {
                Data = Enc.GetBytes(data);
            }
            public override byte[] get()
            {
                return Data;
            }
        }

        class DecimalConverter : AStringConverter<Decimal>
        {
            public override void parse(string data)
            {
                Decimal.TryParse(data, out Data);
            }
            public override Decimal get()
            {
                return Data;
            }
        }
    }
}
