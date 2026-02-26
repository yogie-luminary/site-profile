// Code Simplification Example
// This file demonstrates simplifying redundant conditional logic in C#

namespace CodeSimplification
{
    public class ProgramHeaderExample
    {
        // ORIGINAL CODE (Complex with redundant conditionals)
        public void OriginalCode(bool useWidgetData, dynamic programHeader)
        {
            var useVideoBackground = useWidgetData ? programHeader.UseVideoAsBackground : programHeader.Program.UseVideoAsBackground;
            var imageUrl = useWidgetData ?
                           ! programHeader.UseVideoAsBackground ? programHeader.Image.GetImageOrDefaultUrl() : string.Empty 
                           : ! programHeader.Program.UseVideoAsBackground ? programHeader.Program.Image.GetImageOrDefaultUrl() : string.Empty;
            var videoUrl = useWidgetData ?
                           programHeader.UseVideoAsBackground ? programHeader.Video.Src : string.Empty
                           : programHeader.Program.UseVideoAsBackground ? programHeader.Program.Video.Src : string.Empty;
        }

        // SIMPLIFIED CODE (Clean and maintainable)
        public void SimplifiedCode(bool useWidgetData, dynamic programHeader)
        {
            // Extract the data source once based on the condition
            var dataSource = useWidgetData ? programHeader : programHeader.Program;
            
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
