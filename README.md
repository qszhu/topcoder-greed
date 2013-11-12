Greed
=====

The missing topcoder arena plugin for algorithm contest.

Greed is good
-------------

* No CodeProcessor+FileEdit+blah, no tons of jars, just **Greed**
* Dead simple configuration, just set your **workspace**, end of story
* Keeping stuff in workspace enabling seamless migration between OS and environments
* Testing code. Unit-testing code. Reading-data testing code.
* Fully **customizable**
  - File structure
  - Code generation
  - Templates, templates, templates. You define, I generate!
* Multi language support, including
  - Java
  - C++
  - C#, thanks to @jbransen
  - Python, thanks to @wookayin

Downloads
---------

The versions have been put into the `dist` directory in the repository.

* [2.0-beta](https://github.com/shivawu/topcoder-greed/raw/master/dist/Greed-2.0-beta.jar)
* Legacy [1.5](https://github.com/shivawu/topcoder-greed/raw/master/dist/Greed-1.5.jar)  
  Note that the 2.0 is not compatiable with 1.x versions. Latest versions are recommended.

Release Note
------------
#### 2.0-beta

Great news! **2.0** is finally feature complete and available for public beta test.
Special thanks to @vexorian.

The new features includes:

- Flexible template definition. Rather than sticking to the old source, test, unittest pattern, you can define all kinds of templates yourself, testcase, makefile, scripts. Go wild with your imagination.
- External scripts execution after template rendering
- New configuration schema, described [here](https://github.com/shivawu/topcoder-greed/raw/master/dist/Greed-2.0-beta.jar)
- Shipped with problem statement and test data file generation, with well defined schema
- New default templates, **filetest**. Reading data from sample files and run test. Release your code from the mess of data.
- Various new string renderer, preparing to get templated
- New colorful UI and message

#### 1.5

* Python support. Thanks to @wookayin, now Greed is one of the first plugins who support Python!

#### 1.4

* Major template bug fix , thanks to @ashashwat
* Template bug when allocating large space, thanks to @wookayin

#### 1.3

* unit test code generation for C#(NUnit) and Java(JUnit), thanks to @tomtung
* major rewriting

#### 1.2

* minor bug fix, mainly fix bugs on "long long" in C++, C#, and Java

#### 1.1

* update C# version, thanks to @jbransen
* major bug fix

Quick start
-----------
1. Go to [Downloads], and download the single Greed jar

2. Open __Topcoder arena__ -> __Login__ -> __Options__ -> __Editor__ -> __Add__  
![Add greed](https://github.com/shivawu/topcoder-greed/wiki/images/Add-Plugin.png)<br/>
Done! Remember to check __Default__ and __At startup__.

3. Click __Configure__.  
![Configure greed](https://github.com/shivawu/topcoder-greed/wiki/images/Set-Workspace.png)  
Fill in your workspace full path, make sure it's a existing directory.

4. All set! Go get your rating!
![Greed UI](https://github.com/shivawu/topcoder-greed/wiki/images/UI-Look.png)  


Go rock with config
-------------------

_**Everything in greed is configuable.**_

Greed is bundled with some default config, which should be enough for most of you. But if you're not satisfied, go set.

Start with creating a file called `greed.conf` under your workspace root.

Things you can do with this config, 

### greed.codeRoot

Change where your code is stored, via `greed.codeRoot = ???`, this path is relative to your workspace root.
Default set to `.`, which means workspace root.

### greed.language.\<lang\>

This is the configuration object for a specific language, including its
template definitions, template to use when submitting, and other language specific
settings.

Available language keys are `cpp`, `java`, `csharp`, and `python`.

#### Templates sequence

The key is `greed.language.<lang>.templates`.
This is the template sequence generated by Greed, one after another. 
They remain sequencial since later templates can rely on earlier ones and use their results.

The default templates for each language are `[filetest, source, testcase, problem-desc]`.

* The `filetest` template uses **filetest** templates, which reads data from the `testcase` output, and test your program with them. Bind to a piece of code in the `source` template.
* The `source` template generates class and method signature from the problem definition
* The `testcase` template outputs test data file to `${Problem.Name}.sample`.  
  Pay attention to its format since it's important when adding your own testcase.
  - Each testcase is identified by a labeling line which starts with `--`
  - Each line represents a parameter or the output.
  - If the data itself is an array, there should be multiple lines.
    Leading with a line with its length, and following by several lines with each of its content.
  - The input and output are separated by an empty line. (Actually, this line is ignored no matter what's in it, you can see its details in the **filetest** templates).
* The `problem-desc` template generates the problem statement into a web page.

#### Unit test

Another official template is `unittest`, but disabled by default.
This kind of template generates unit test code leveraging UT framework like `junit` for Java and `nunit` for C#.
To use it, set as the following (only available for Java and C#):

```
greed.language.<lang>.templates = [source, unittest, problem-desc]
```

This idea was orignally proposed by @tomtung.
You can use the unittest code in your favorite IDE, like the following

![Resharper NUnit Debugging](https://raw.github.com/wiki/shivawu/topcoder-greed/images/Resharper-NUnit-Debug.png)

#### Legacy test

Older versions of Greed write test code and test data into code, which makes the generated
code a bit messy. However, it's left for backward compatibility and users who actually like it.

```
greed.language.<lang>.templates = [test, source, problem-desc]
```

### Template definition

Magic here!

You can define your own template type, using the `templateDef` key 
under `greed.language.<lang>`. 

Here's what you're going to do.

- Specify the `override` behaviour
- Set the `outputKey` or `outputFile`, using variables like `${Problem.Name}`, `${Contest.Name}`
- **Write your awesome template**
- Set the `templateFile` to your template (relative path to your workspace)
- If you like, use some code transformers to cleanup your translated code

You can also add `afterFileGen` actions to execute scripts or programs after 
the template generation. Extreme flexibility is given.
Here's an possible example of the `testcase` template def.

```
greed.language.cpp.templateDef {
    testcase {
        override = false
        outputFile = "${Contest.Name}/${Problem.Name}.data"
        templateFile = "builtin testcase/testcases.tmpl"
        afterFileGen {
            execute = /usr/bin/python
            arguments = [ ./split.py, "${GeneratedFileName}", "${Contest.Name}/data" ]
        }
    }
}
```

Remember to add your template key (`testcase` in this example) to the `templates` sequence 
for this language.

Want to learn more?
-------------------
Go to [wiki](https://github.com/shivawu/topcoder-greed/wiki). 

You'll learn how to config all the functionalities, like

* How to write your own templates (Yeh!) and provided templates
* All available binded info in config, like `${Problem.Score}`
* Setting the `begin cut` and `end cut` format and other language specific configurations

Bug Tracker
-----------
When you found a bug, or need a new feature, raise an issue [here](https://github.com/shivawu/topcoder-greed/issues).
Please, with the problem you're solving and the room you're in, to better identify the problem.

Or, consider contribution!

Contribute to me
----------------

Any help is helpful and greatly appreciated.

You can contribute in 2 ways:

1. Fork, code, and send a pull request. Oh, you're not familiar with this style, well, you should. Read [this article](https://help.github.com/articles/fork-a-repo).
2. Be a collaborator, contact @shivawu

License
-------

Copyright 2012-2013 Greed

Licensed under _Apache License, Version 2.0_. You may obtain a copy of the license in the _LICENSE_ file, or at:

[http://www.apache.org/licenses/LICENSE-2.0]()

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
