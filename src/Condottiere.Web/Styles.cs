public class Styles
{
    public const string MainColor = "red";
    public const string Active = "text-red-900 bg-white hover:bg-red-50";
    public const string DefaultIcon = "w-4 h-4 md:w-6 md:h-6";
    public const string Link = "text-xs font-bold text-indigo-600 hover:text-indigo-800 lg:text-base";
    public const string Title = $"text-red-700 font-semibold uppercase text-xl xl:text-4xl";
    public const string SubTitle = $"text-sm text-red-600 tracking-widest lg:text-lg";
    public const string Container = $"h-full flex justify-center py-8";
    public const string PrimaryButton = "w-full bg-red-700  hover:bg-red-500 text-white " + button;
    public const string SecondaryButton = "w-full bg-red-200 hover:bg-red-300 text-red-700 " + button;
    public const string PrimaryInlineButton = "bg-red-700  hover:bg-red-500 text-white " + button;
    public const string SecondaryInlineButton = "bg-red-200 hover:bg-red-300 text-red-700 " + button;
    private const string button = $"font-semibold outline-none focus:outline-none rounded-xl " +
        $"p-2 md:p-4 md:text-xl md:text-2xl uppercase outline-none focus:outline-none flex items-center justify-center";
    public const string Input = "font-semibold w-full block px-2 py-1.5 border border-red-400 rounded-md text-sm shadow-sm focus:outline-none focus:border-red-600";
    public const string ValidationError = "pt-1 color-red-500 text-xs font-semibold";
}
