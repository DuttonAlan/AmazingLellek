using System.Collections.Specialized;
using AutoMapper;

namespace PlanB.DPF.Manager.Function.Mapping;

/// <summary>
/// Profile class for the Automapper.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    //public MappingProfile()
    //{
    //    // mapping of internal classes
    //    CreateMap<IdValueMockModel, IdValueDto>()
    //        .ForMember(target => target.Id, source => source.MapFrom(source => source.DatabaseId))
    //        .ForMember(target => target.Value, source => source.MapFrom(source => source.DatabaseValue))
    //        .ReverseMap();
        
    //    CreateMap<NameValueCollection, LookupQuery>()
    //        .ForMember(target => target.Type, source => source.MapFrom(source => GetsourceListValuesAsString(source, "types")));
    //}
    
    
    /// <summary>
    /// Gets a list of values of the given ICollection.
    /// </summary>  
    /// <param name="source"></param>
    /// <param name="member"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception">If the type of the object is no generic type.</exception>
    private static List<string> GetsourceListValuesAsString(NameValueCollection source, string member)
    {
        var values = source[member] != null ? source[member].Split(",").ToList() : new List<string>();
        return values;
    }
    
}