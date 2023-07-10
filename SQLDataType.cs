public class SQLDataType
{
    public string TypeName { get; }
    public int? Length { get; }
    public bool IsNullable { get; set; }
    public bool IsIdentity { get; set; }

    public SQLDataType(string typeName, int? length = null, bool isNullable = false, bool isIdentity = false)
    {
        TypeName = typeName;
        Length = length;
        IsNullable = isNullable;
        IsIdentity = isIdentity;
    }

    public static implicit operator SQLDataType(string value)
    {
        return new SQLDataType(value);
    }
    public static implicit operator SQLDataType((string, int) tuple)
    {
        return new SQLDataType(tuple.Item1, tuple.Item2);
    }
    public static implicit operator SQLDataType((string, bool) tuple)
    {
        return new SQLDataType(tuple.Item1, isNullable: tuple.Item2);
    }
    public static implicit operator SQLDataType((string, int,bool) tuple)
    {
        return new SQLDataType(tuple.Item1, tuple.Item2, tuple.Item3);
    }

    public static implicit operator string(SQLDataType dataType)
    {
        return dataType.TypeName;
    }

    public override string ToString()
    {
        return TypeName;
    }

    /// <summary> Creates a nullable copy of this object </summary>
    public SQLDataType Nullable()
    {
        SQLDataType newType = this;
        newType.IsNullable = true;
        return newType;
    }

    public SQLDataType Identity()
    {
        SQLDataType newType = this;
        newType.IsIdentity = true;
        return newType;
    }

    // MSSQL data types
    public static SQLDataType Char(int Length) { return new SQLDataType("Char", Length); }
    public static SQLDataType Varchar(int length) { return new SQLDataType("Varchar", length); }
    public static readonly SQLDataType BigInt = "BigInt";
    public static readonly SQLDataType Binary = "Binary";
    public static readonly SQLDataType Bit = "Bit";
    public static readonly SQLDataType Date = "Date";
    public static readonly SQLDataType DateTime = "DateTime";
    public static readonly SQLDataType Decimal = "Decimal";
    public static readonly SQLDataType Float = "Float";
    public static readonly SQLDataType Image = "Image";
    public static readonly SQLDataType Int = "Int";
    public static readonly SQLDataType Money = "Money";
    public static readonly SQLDataType NChar = "NChar";
    public static readonly SQLDataType NText = "NText";
    public static readonly SQLDataType Numeric = "Numeric";
    public static readonly SQLDataType NVarchar = "NVarchar";
    public static readonly SQLDataType Real = "Real";
    public static readonly SQLDataType SmallDateTime = "SmallDateTime";
    public static readonly SQLDataType SmallInt = "SmallInt";
    public static readonly SQLDataType SmallMoney = "SmallMoney";
    public static readonly SQLDataType Text = "Text";
    public static readonly SQLDataType Time = "Time";
    public static readonly SQLDataType Timestamp = "Timestamp";
    public static readonly SQLDataType TinyInt = "TinyInt";
    public static readonly SQLDataType UniqueIdentifier = "UniqueIdentifier";
    public static readonly SQLDataType VarBinary = "VarBinary";
    public static readonly SQLDataType VarcharMax = "Varchar(max)";
    public static readonly SQLDataType XML = "XML";
}
