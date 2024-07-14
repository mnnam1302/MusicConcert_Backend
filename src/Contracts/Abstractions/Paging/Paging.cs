namespace Contracts.Abstractions.Paging;

public record Paging
{
    private const int UpperLimit = 100;
    private const int DefaultLimit = 10;
    private const int DefaultOffset = 0;

    public int Page { get; init; }
    public int PageSize { get; init; }

    public Paging(int page, int pageSize)
    {
        Page = page < 1 ? DefaultOffset : page;
        PageSize = pageSize < 1
            ? DefaultLimit
            : pageSize > 100
            ? UpperLimit
            : pageSize;
    }

    // gRPC later
    //public static implicit operator Paging(Protobuf.Paging paging)
    //    => new(paging.Limit, paging.Offset);

    //public static implicit operator Protobuf.Paging(Paging paging)
    //    => new() { Limit = paging.Limit, Offset = paging.Offset };
}