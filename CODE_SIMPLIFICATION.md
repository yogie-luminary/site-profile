# C# Code Simplification Guide

## Overview

This document demonstrates how to simplify redundant conditional logic in C# code by applying the DRY (Don't Repeat Yourself) principle.

## Problem Statement

The original code contains redundant conditional checks that make it harder to read, maintain, and modify:

```csharp
var useVideoBackground = useWidgetData ? programHeader.UseVideoAsBackground : programHeader.Program.UseVideoAsBackground;
var imageUrl = useWidgetData ?
               ! programHeader.UseVideoAsBackground ? programHeader.Image.GetImageOrDefaultUrl() : string.Empty 
               : ! programHeader.Program.UseVideoAsBackground ? programHeader.Program.Image.GetImageOrDefaultUrl() : string.Empty;
var videoUrl = useWidgetData ?
               programHeader.UseVideoAsBackground ? programHeader.Video.Src : string.Empty
               : programHeader.Program.UseVideoAsBackground ? programHeader.Program.Video.Src : string.Empty;
```

### Issues with the Original Code:

1. **Redundant Conditionals**: The `useWidgetData` condition is evaluated three separate times
2. **Duplicate Property Access**: `programHeader.UseVideoAsBackground` and `programHeader.Program.UseVideoAsBackground` are accessed multiple times
3. **Poor Readability**: Nested ternary operators make the code difficult to understand
4. **Maintenance Risk**: If the logic needs to change, multiple locations must be updated
5. **Copy-Paste Errors**: Easy to make mistakes when duplicating similar conditional patterns

## Solution

Extract the data source selection into a single variable, then use it consistently:

```csharp
// Extract the data source once based on the condition
var dataSource = useWidgetData ? programHeader : programHeader.Program;

// Now use the data source consistently
var useVideoBackground = dataSource.UseVideoAsBackground;
var imageUrl = !useVideoBackground ? dataSource.Image.GetImageOrDefaultUrl() : string.Empty;
var videoUrl = useVideoBackground ? dataSource.Video.Src : string.Empty;
```

## Benefits of the Simplified Approach

| Aspect | Before | After |
|--------|--------|-------|
| **Lines of Code** | 5 lines | 5 lines (but much clearer) |
| **Conditional Checks** | 3 checks on `useWidgetData` | 1 check on `useWidgetData` |
| **Property Access** | Multiple redundant accesses | Single access per property |
| **Readability** | Complex nested ternaries | Clear, linear flow |
| **Maintainability** | Changes needed in 3 places | Changes needed in 1 place |
| **Bug Risk** | High (easy to make copy-paste errors) | Low (single source of truth) |

## Key Principles Applied

### 1. DRY (Don't Repeat Yourself)
By extracting the conditional logic once, we avoid repeating the same pattern multiple times.

### 2. Single Responsibility
Each variable now has a clear, single purpose without mixing conditional logic.

### 3. Separation of Concerns
Data source selection is separated from the usage of that data.

### 4. Readability
The simplified code reads like natural language: "Get the data source, then use it."

## Verification of Equivalence

Both code versions produce identical results:

### Case 1: useWidgetData = true
- **Data source**: `programHeader`
- **useVideoBackground**: `programHeader.UseVideoAsBackground`
- **imageUrl**: Empty if using video, else `programHeader.Image.GetImageOrDefaultUrl()`
- **videoUrl**: `programHeader.Video.Src` if using video, else empty

### Case 2: useWidgetData = false
- **Data source**: `programHeader.Program`
- **useVideoBackground**: `programHeader.Program.UseVideoAsBackground`
- **imageUrl**: Empty if using video, else `programHeader.Program.Image.GetImageOrDefaultUrl()`
- **videoUrl**: `programHeader.Program.Video.Src` if using video, else empty

## Best Practices for Similar Refactoring

1. **Identify the Pattern**: Look for repeated conditional checks on the same variable
2. **Extract Common Logic**: Move the conditional to a single location
3. **Use Descriptive Names**: Choose variable names that clearly indicate their purpose
4. **Test Thoroughly**: Ensure the refactored code behaves identically to the original
5. **Document the Change**: Explain why the simplification improves the code

## Conclusion

This refactoring demonstrates how a small structural change can significantly improve code quality. By eliminating redundancy and improving readability, we make the codebase easier to understand, maintain, and extend.

## See Also

- [CodeSimplification.cs](./CodeSimplification.cs) - Working example with both versions
