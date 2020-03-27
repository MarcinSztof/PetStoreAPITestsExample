namespace APIClient.Models
{
    public enum Anonymous
    {
        [System.Runtime.Serialization.EnumMember(Value = @"available")]
        Available = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"pending")]
        Pending = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"sold")]
        Sold = 2,

    }

    public enum PetStatus
    {
        [System.Runtime.Serialization.EnumMember(Value = @"available")]
        Available = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"pending")]
        Pending = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"sold")]
        Sold = 2,

    }

    public enum OrderStatus
    {
        [System.Runtime.Serialization.EnumMember(Value = @"placed")]
        Placed = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"approved")]
        Approved = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"delivered")]
        Delivered = 2,

    }
}
