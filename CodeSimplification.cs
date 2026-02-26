// Code Simplification Example
// This file demonstrates simplifying redundant conditional logic in C#

namespace CodeSimplification
{
    // Interface to ensure type safety instead of using dynamic
    public interface IMediaSource
    {
        bool UseVideoAsBackground { get; }
        IImageData Image { get; }
        IVideoData Video { get; }
    }

    public interface IImageData
    {
        string GetImageOrDefaultUrl();
    }

    public interface IVideoData
    {
        string Src { get; }
    }

    public class ProgramHeaderExample
    {
        // ORIGINAL CODE (Complex with redundant conditionals)
        // Note: Using IMediaSource for the example. In real code, you might have
        // a ProgramHeader class with a Program property of the same type.
        public void OriginalCode(bool useWidgetData, IMediaSource programHeader, IMediaSource program)
        {
            var useVideoBackground = useWidgetData ? programHeader.UseVideoAsBackground : program.UseVideoAsBackground;
            var imageUrl = useWidgetData ?
                           ! programHeader.UseVideoAsBackground ? programHeader.Image.GetImageOrDefaultUrl() : string.Empty 
                           : ! program.UseVideoAsBackground ? program.Image.GetImageOrDefaultUrl() : string.Empty;
            var videoUrl = useWidgetData ?
                           programHeader.UseVideoAsBackground ? programHeader.Video.Src : string.Empty
                           : program.UseVideoAsBackground ? program.Video.Src : string.Empty;
        }

        // SIMPLIFIED CODE (Clean and maintainable)
        public void SimplifiedCode(bool useWidgetData, IMediaSource programHeader, IMediaSource program)
        {
            // Extract the data source once based on the condition
            var dataSource = useWidgetData ? programHeader : program;
            
            // Now use the data source consistently
            var useVideoBackground = dataSource.UseVideoAsBackground;
            var imageUrl = !useVideoBackground ? dataSource.Image.GetImageOrDefaultUrl() : string.Empty;
            var videoUrl = useVideoBackground ? dataSource.Video.Src : string.Empty;
        }
    }
}

/* 
 * BENEFITS OF SIMPLIFICATION:
 * 
 * 1. Reduced Redundancy: The condition 'useWidgetData' is evaluated only once instead of three times
 * 2. Better Readability: The code is much easier to read and understand
 * 3. Easier Maintenance: Changes to the logic need to be made in only one place
 * 4. Fewer Bugs: Less duplication means less chance of copy-paste errors
 * 5. DRY Principle: Follows "Don't Repeat Yourself" principle
 */
