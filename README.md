# File Organizer Tool

A simple yet powerful C# console application that automatically organizes files into categorized folders based on their file extensions.

## Features

- **Automatic Categorization**: Sorts files into predefined categories (Images, Documents, Audio, Video, Archives, Code, Executables, and Others)
- **Flexible Operation Modes**: Choose to either move or copy files
- **Subfolder Support**: Option to include files from subfolders
- **Duplicate Handling**: Automatically renames duplicate files to avoid conflicts
- **Smart Skip Logic**: Skips files that are already in category folders
- **Detailed Summary**: Displays statistics showing how many files were processed in each category
- **Colorful Console Output**: Easy-to-read color-coded feedback

## Supported File Types

### Images
`.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.svg`, `.ico`, `.webp`

### Documents
`.pdf`, `.doc`, `.docx`, `.txt`, `.xlsx`, `.xls`, `.ppt`, `.pptx`, `.csv`, `.odt`

### Audio
`.mp3`, `.wav`, `.flac`, `.aac`, `.ogg`, `.m4a`, `.wma`

### Video
`.mp4`, `.avi`, `.mkv`, `.mov`, `.wmv`, `.flv`, `.webm`, `.m4v`

### Archives
`.zip`, `.rar`, `.7z`, `.tar`, `.gz`, `.iso`

### Code
`.cs`, `.java`, `.py`, `.js`, `.html`, `.css`, `.cpp`, `.c`, `.h`, `.json`, `.xml`

### Executables
`.exe`, `.msi`, `.dll`, `.bat`, `.sh`

## How to Use

1. **Run the application**
   ```bash
   dotnet run
   ```

2. **Enter the folder path** you want to organize when prompted
   - Leave blank to use the current directory

3. **Choose operation mode**:
   - `1` - Move files (original files will be relocated)
   - `2` - Copy files (original files remain in place)

4. **Include subfolders?**
   - `y` - Process files in all subfolders
   - `n` - Process only files in the main folder

5. **Wait for completion** and review the summary statistics

## Example

```
╔═══════════════════════════════════════╗
║          FILE ORGANIZER TOOL          ║
╚═══════════════════════════════════════╝

Enter the folder path that you would like to organize:
C:\Users\YourName\Downloads

Choose operation mode:
1. Move files (original files will be moved)
2. Copy files (original files remain)

Enter your choice (1 or 2): 1

Include subfolders? (y/n): n

Moving files...

found 47 files

MOVED: vacation.jpg → Images/
MOVED: report.pdf → Documents/
MOVED: song.mp3 → Audio/
...

──────────────────────────────────────────────────
SUMMARY:
──────────────────────────────────────────────────
Images               :    15 file(s)
Documents            :    12 file(s)
Audio                :     8 file(s)
Video                :     5 file(s)
Archives             :     4 file(s)
Code                 :     2 file(s)
Others               :     1 file(s)
──────────────────────────────────────────────────
Total Processed      :    47 file(s)

All files organized successfully!
```

## Requirements

- .NET 5.0 or higher
- Windows, Linux, or macOS

## Building from Source

```bash
# Clone or download the repository
# Navigate to the project directory
cd File_Organizational_Tool

# Build the project
dotnet build

# Run the application
dotnet run
```

## Error Handling

The application includes comprehensive error handling:
- Validates folder existence before processing
- Catches and displays file operation errors
- Reports error count in the summary
- Continues processing remaining files even if some fail

## Notes

- Files already located in category folders will be skipped to prevent redundant organization
- Duplicate filenames are automatically handled by appending a counter (e.g., `file_1.txt`, `file_2.txt`)
- The application will create category folders as needed

## License

This project is open source and available for personal and commercial use.

## Contributing

Feel free to submit issues, fork the repository, and create pull requests for any improvements.
