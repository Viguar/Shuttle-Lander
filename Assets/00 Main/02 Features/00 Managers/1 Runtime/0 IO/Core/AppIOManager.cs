using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;

public class AppIOManager : MonoBehaviour
{
    private RuntimeManager _RuntimeManager;
    public IOFilePaths _FilePathsInfo;
    private static string _ioBaseDirectory;

    public void InitComponent(RuntimeManager runtimeManager)
    {
        _RuntimeManager = runtimeManager;
    }

    private void ResolveBaseDirectory()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                //Standalone Windows
                _ioBaseDirectory = Application.dataPath;
                break;

            case RuntimePlatform.WindowsEditor:
                //Unity Editor Windows
                if (!Directory.Exists(Path.Combine(Application.dataPath, "_Config"))) { Directory.CreateDirectory(Path.Combine(Application.dataPath, "_Config")).ToString(); }
                _ioBaseDirectory = Path.Combine(Application.dataPath, "_Config");
                break;

            default:
                //Any other platform
                _ioBaseDirectory = Application.persistentDataPath;
                break;
        }
    }
    private void ResolveRequestedDirectory(string dir)
    {
        if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
    }


    //this method needs improvement because currently its the filename that makes it weak. it currently should never be duplicate.
    //exports anythings though!
    public void ExportToFile<T>(T obj, string ioFile)
    {
        ResolveBaseDirectory();
        if (_FilePathsInfo._AppCustomFilePaths.Any(filePath => filePath._FileName == ioFile))
        {            
            //Create the requested full filepath
            CustomFilePaths fileInfo = _FilePathsInfo._AppCustomFilePaths.Find(data => data._FileName == ioFile);
            string fileExtension = _FilePathsInfo._CustomExtension ? _FilePathsInfo._AppFilesExtension : ".json";
            ResolveRequestedDirectory(Path.Combine(_ioBaseDirectory + fileInfo._FilePath));
            string filePath = Path.Combine(_ioBaseDirectory + fileInfo._FilePath, fileInfo._FileName + fileExtension);
            
            try
            {
                var jsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                string jsonString = JsonConvert.SerializeObject(obj, jsonSettings);
                File.WriteAllText(filePath, jsonString);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to export data to JSON: {ex.Message}");
            }
        }
    }

    //Call by other classes needing to read a file.
    public T ImportFromFile<T>(string ioFile)
    {
        ResolveBaseDirectory();
        if (_FilePathsInfo._AppCustomFilePaths.Any(filePath => filePath._FileName == ioFile))
        {
            //Create the requested full filepath
            CustomFilePaths fileInfo = _FilePathsInfo._AppCustomFilePaths.Find(data => data._FileName == ioFile);
            string fileExtension = _FilePathsInfo._CustomExtension ? _FilePathsInfo._AppFilesExtension : ".json";
            ResolveRequestedDirectory(Path.Combine(_ioBaseDirectory + fileInfo._FilePath));
            string filePath = Path.Combine(_ioBaseDirectory + fileInfo._FilePath, fileInfo._FileName + fileExtension);
            
            try
            {
                string fileAsString = File.ReadAllText(filePath);
                T obj = JsonConvert.DeserializeObject<T>(fileAsString);
                return obj;
            }
            catch (IOException e)
            {
                Debug.LogError($"Error reading file: {e.Message}");
                return default;
            }
            catch (JsonException e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
                return default;
            }
        }
        else
        {
            Debug.LogError($"File not found: {ioFile}");
            return default;
        }
    }

    public bool FileExists(string ioFile)
    {
        ResolveBaseDirectory();
        if (_FilePathsInfo._AppCustomFilePaths.Any(filePath => filePath._FileName == ioFile))
        {
            // Create the requested full file path
            CustomFilePaths fileInfo = _FilePathsInfo._AppCustomFilePaths.Find(data => data._FileName == ioFile);
            string fileExtension = _FilePathsInfo._CustomExtension ? _FilePathsInfo._AppFilesExtension : ".json";
            ResolveRequestedDirectory(Path.Combine(_ioBaseDirectory + fileInfo._FilePath));
            string filePath = Path.Combine(_ioBaseDirectory + fileInfo._FilePath, fileInfo._FileName + fileExtension);

            // Check if the file exists
            return File.Exists(filePath);
        }
        else
        {
            return false;
        }
    }
}