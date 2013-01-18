IRubyRunner
----------
This is a simple application that runs an iron ruby compatible script.

Usage
-----
`c:\>RubyRunner.exe filename [arguments]`

**filename**: The path to the iron ruby compatible script.  
**arguments**: arguments to pass to the script

Usage Example
-------------
`c:\>RubyRunner.exe path\to\script.rb arg -another arg`

**Note**: The way it passes the arguments to the ruby script is by adding each of them to the ARGV ruby variable (ARGV << argument)

**Note**: It does not have any of the Standard Library scripts, so you'll need to add those manually (in code) to your $LOAD_PATH variable and `require` them appropiately.

**Example**

````
$LOAD_PATH << ".\Lib"

require "System"
````