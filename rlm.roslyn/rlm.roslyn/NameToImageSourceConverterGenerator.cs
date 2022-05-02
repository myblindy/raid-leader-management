using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace rlm.roslyn;

[Generator]
class NameToImageSourceConverterGenerator : IIncrementalGenerator
{
    const string weaponTypeName = "WeaponType";
    const string weaponTypePathName = "WeaponTypes";

    const string rolesTypeName = "Roles";
    const string rolesTypePathName = "Roles";

    const string noneName = "None";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var weaponTypeValuesProvider = context.SyntaxProvider.CreateSyntaxProvider(
            (n, ct) => n.IsKind(SyntaxKind.EnumDeclaration) && ((EnumDeclarationSyntax)n).Identifier.Text is weaponTypeName,
            (ctx, ct) => ((EnumDeclarationSyntax)ctx.Node).Members.Select(m => m.Identifier.Text).ToImmutableArray());

        var rolesValuesProvider = context.SyntaxProvider.CreateSyntaxProvider(
            (n, ct) => n.IsKind(SyntaxKind.EnumDeclaration) && ((EnumDeclarationSyntax)n).Identifier.Text is rolesTypeName,
            (ctx, ct) => ((EnumDeclarationSyntax)ctx.Node).Members.Select(m => m.Identifier.Text).ToImmutableArray());

        var combinedValuesProvider = weaponTypeValuesProvider.Combine(rolesValuesProvider.Collect())
            .Select((pair, ct) => (weaponTypeValues: pair.Left, rolesValues: pair.Right.SelectMany(w => w).ToImmutableArray()));

        context.RegisterSourceOutput(combinedValuesProvider, Execute);
    }

    static void Execute(SourceProductionContext context, (ImmutableArray<string> weaponType, ImmutableArray<string> roles) values)
    {
        var sb = new StringBuilder()
            .AppendLine(@"
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using rlm.Models;

namespace rlm.Converters;

class NameToImageSourceConverter : IValueConverter
{
    static readonly string ApplicationPath = AppContext.BaseDirectory;
    static readonly Dictionary<(string Name, string Type), BitmapImage> CustomBitmapImageCache = new();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is string valueString  && parameter is string typeString 
            ? CustomBitmapImageCache.TryGetValue((valueString, typeString), out var image) ? image 
                : CustomBitmapImageCache[(valueString, typeString)] = new(new(Path.Combine(ApplicationPath, $@""Data\Images\{typeString}\{valueString}.png"")))
            : value switch 
            {");

        // converter switch
        foreach (var weaponTypeValue in values.weaponType.Where(n => n is not noneName))
            sb.AppendLine($"{weaponTypeName}.{weaponTypeValue} => {weaponTypeName}{weaponTypeValue}Bitmap,");
        sb.AppendLine($"{weaponTypeName}.{noneName} => null,");
        foreach (var roleValue in values.roles.Where(n => n is not noneName))
            sb.AppendLine($"{rolesTypeName}.{roleValue} => {rolesTypeName}{roleValue}Bitmap,");

        sb.AppendLine("_ => throw new NotImplementedException()")
            .AppendLine("};")
            .AppendLine("public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();");

        // static bitmaps
        foreach (var weaponTypeValue in values.weaponType.Where(n => n is not noneName))
            sb.AppendLine($@"static readonly BitmapImage {weaponTypeName}{weaponTypeValue}Bitmap = new(new(Path.Combine(ApplicationPath, @""Data\Images\{weaponTypePathName}\{weaponTypeValue}.png"")));");
        foreach (var roleValue in values.roles.Where(n => n is not noneName))
            sb.AppendLine($@"static readonly BitmapImage {rolesTypeName}{roleValue}Bitmap = new(new(Path.Combine(ApplicationPath, @""Data\Images\{rolesTypePathName}\{roleValue}.png"")));");

        sb.AppendLine("}");

        context.AddSource("NameToImageSourceConverter.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }
}
