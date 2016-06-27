# Zipper
A Zip program that zip specific files defined by a *.zs file.

## Solve what problem?
Sometimes you just want to pack particular files and directories in a folder and do it everyday, but most zip software do not provide such solution.

So I made this one, which works similar to a batch file that compress specific files and folders. See below about the setting file.


## A zipper setting file:
In a zipper setting file, you can specify which dir and file to exclude or include.
The following is a *.zs file.
```json
{
    "ignoredir":[".vs","ng2ts/.git","ng2ts/node_modules"],
    "ignorefile":["ng2ts/.gitattributes","ng2ts/.gitignore"],
    "includedir":[],
    "includefile":[]
}
```
It will avoid compressing ".vs","ng2ts/.git","ng2ts/node_modules" directories and "ng2ts/.gitattributes","ng2ts/.gitignore" files that sit in its folder.
