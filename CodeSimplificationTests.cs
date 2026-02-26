// Unit Tests for Code Simplification
// This demonstrates that both versions produce identical results

using System;

namespace CodeSimplification.Tests
{
    // Mock classes to support the example
    public class ImageData
    {
        public string Url { get; set; }
        public string GetImageOrDefaultUrl() => Url ?? "default-image.jpg";
    }

    public class VideoData
    {
        public string Src { get; set; }
    }

    public class ProgramData
    {
        public bool UseVideoAsBackground { get; set; }
        public ImageData Image { get; set; }
        public VideoData Video { get; set; }
    }

    public class ProgramHeader
    {
        public bool UseVideoAsBackground { get; set; }
        public ImageData Image { get; set; }
        public VideoData Video { get; set; }
        public ProgramData Program { get; set; }
    }

    public class SimplificationTests
    {
        public static void Main()
        {
            Console.WriteLine("Testing Code Simplification Equivalence\n");
            
            // Test Case 1: useWidgetData = true, UseVideoAsBackground = true
            TestCase("Test 1: Widget data, video background", 
                     useWidgetData: true, 
                     widgetVideoMode: true, 
                     programVideoMode: false);
            
            // Test Case 2: useWidgetData = true, UseVideoAsBackground = false
            TestCase("Test 2: Widget data, image background", 
                     useWidgetData: true, 
                     widgetVideoMode: false, 
                     programVideoMode: false);
            
            // Test Case 3: useWidgetData = false, UseVideoAsBackground = true
            TestCase("Test 3: Program data, video background", 
                     useWidgetData: false, 
                     widgetVideoMode: false, 
                     programVideoMode: true);
            
            // Test Case 4: useWidgetData = false, UseVideoAsBackground = false
            TestCase("Test 4: Program data, image background", 
                     useWidgetData: false, 
                     widgetVideoMode: false, 
                     programVideoMode: false);
            
            Console.WriteLine("\n✓ All tests passed! Both implementations are equivalent.");
        }

        private static void TestCase(string testName, bool useWidgetData, bool widgetVideoMode, bool programVideoMode)
        {
            Console.WriteLine($"\n{testName}");
            Console.WriteLine("=".PadRight(60, '='));
            
            // Setup test data
            var programHeader = new ProgramHeader
            {
                UseVideoAsBackground = widgetVideoMode,
                Image = new ImageData { Url = "widget-image.jpg" },
                Video = new VideoData { Src = "widget-video.mp4" },
                Program = new ProgramData
                {
                    UseVideoAsBackground = programVideoMode,
                    Image = new ImageData { Url = "program-image.jpg" },
                    Video = new VideoData { Src = "program-video.mp4" }
                }
            };

            // Original implementation
            var original = OriginalImplementation(useWidgetData, programHeader);
            
            // Simplified implementation
            var simplified = SimplifiedImplementation(useWidgetData, programHeader);
            
            // Verify results match
            bool resultsMatch = 
                original.useVideoBackground == simplified.useVideoBackground &&
                original.imageUrl == simplified.imageUrl &&
                original.videoUrl == simplified.videoUrl;
            
            Console.WriteLine($"Original   - UseVideo: {original.useVideoBackground}, ImageUrl: {original.imageUrl}, VideoUrl: {original.videoUrl}");
            Console.WriteLine($"Simplified - UseVideo: {simplified.useVideoBackground}, ImageUrl: {simplified.imageUrl}, VideoUrl: {simplified.videoUrl}");
            Console.WriteLine($"Match: {(resultsMatch ? "✓ PASS" : "✗ FAIL")}");
            
            if (!resultsMatch)
            {
                throw new Exception($"Test failed: {testName}");
            }
        }

        private static (bool useVideoBackground, string imageUrl, string videoUrl) 
            OriginalImplementation(bool useWidgetData, ProgramHeader programHeader)
        {
            var useVideoBackground = useWidgetData ? programHeader.UseVideoAsBackground : programHeader.Program.UseVideoAsBackground;
            var imageUrl = useWidgetData ?
                           ! programHeader.UseVideoAsBackground ? programHeader.Image.GetImageOrDefaultUrl() : string.Empty 
                           : ! programHeader.Program.UseVideoAsBackground ? programHeader.Program.Image.GetImageOrDefaultUrl() : string.Empty;
            var videoUrl = useWidgetData ?
                           programHeader.UseVideoAsBackground ? programHeader.Video.Src : string.Empty
                           : programHeader.Program.UseVideoAsBackground ? programHeader.Program.Video.Src : string.Empty;
            
            return (useVideoBackground, imageUrl, videoUrl);
        }

        private static (bool useVideoBackground, string imageUrl, string videoUrl) 
            SimplifiedImplementation(bool useWidgetData, ProgramHeader programHeader)
        {
            // Extract the data source once based on the condition
            // Using an interface would be better in production code to avoid this cast
            IMediaSource dataSource = useWidgetData ? 
                (IMediaSource)new MediaSourceAdapter(programHeader) : 
                (IMediaSource)new MediaSourceAdapter(programHeader.Program);
            
            // Now use the data source consistently
            var useVideoBackground = dataSource.UseVideoAsBackground;
            var imageUrl = !useVideoBackground ? dataSource.Image.GetImageOrDefaultUrl() : string.Empty;
            var videoUrl = useVideoBackground ? dataSource.Video.Src : string.Empty;
            
            return (useVideoBackground, imageUrl, videoUrl);
        }
    }

    // Adapter to provide type-safe access without dynamic
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

    public class MediaSourceAdapter : IMediaSource
    {
        private readonly ProgramData _programData;
        private readonly bool _isFromHeader;
        private readonly ProgramHeader _header;

        public MediaSourceAdapter(ProgramData data)
        {
            _programData = data;
            _isFromHeader = false;
        }

        public MediaSourceAdapter(ProgramHeader header)
        {
            _header = header;
            _isFromHeader = true;
        }

        public bool UseVideoAsBackground => _isFromHeader ? _header.UseVideoAsBackground : _programData.UseVideoAsBackground;
        public IImageData Image => _isFromHeader ? (IImageData)new ImageDataAdapter(_header.Image) : new ImageDataAdapter(_programData.Image);
        public IVideoData Video => _isFromHeader ? (IVideoData)new VideoDataAdapter(_header.Video) : new VideoDataAdapter(_programData.Video);
    }

    public class ImageDataAdapter : IImageData
    {
        private readonly ImageData _imageData;
        public ImageDataAdapter(ImageData data) => _imageData = data;
        public string GetImageOrDefaultUrl() => _imageData.GetImageOrDefaultUrl();
    }

    public class VideoDataAdapter : IVideoData
    {
        private readonly VideoData _videoData;
        public VideoDataAdapter(VideoData data) => _videoData = data;
        public string Src => _videoData.Src;
    }
    }
}
