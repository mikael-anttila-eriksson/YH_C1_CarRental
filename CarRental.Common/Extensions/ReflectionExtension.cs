using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; // ForReflection!!!
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Extensions;
/// <summary>
/// Class to help generic methods with "Reflection".
/// </summary>
public static class ReflectionExtension
{
    public static FieldInfo[] GetVariables(this Type type)
    => type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

    public static FieldInfo? FindCollection<T>(this FieldInfo[] fields) where T : class
        => fields.FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly)
        ?? throw new InvalidOperationException();

    public static object? GetData(this FieldInfo field, object container)
        => field.GetValue(container)
        ?? throw new InvalidDataException();

    /// <summary>
    /// This created some problem when casting as IQueryable. 
    /// Worked better with AsQueryable. Dont know why?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static IQueryable<T> ToQueryable<T>(this object? data) where T : class
        => data is not null && data is List<T>
        ? (IQueryable<T>)data
        : throw new InvalidDataException();

    public static IQueryable<T> myAsQueryable<T>(this object? data) where T : class
        => data is not null && data is List<T>
        ? ((List<T>)data).AsQueryable()
        : throw new InvalidDataException();

    public static List<T> Filter<T>(this IQueryable<T> collection, Func<T, bool>? expLambda)
        => expLambda is null
        ? collection.ToList()
        : collection.Where(expLambda).ToList();

}
