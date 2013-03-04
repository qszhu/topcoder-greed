	static void RunTestcase(int cs) {
		switch (cs) {
			// Your custom testcase goes here
			case -1:
				//DoTest(${foreach Method.Params p , }${p.Name}${end}, expected, cs);
				break;

${<foreach Examples e}
			case ${e.Num}: {
${<foreach e.Input in}
${<if !in.Param.Type.Array}
				${in.Param.Type.Primitive} ${in.Param.Name} = ${in};
${<else}
				${in.Param.Type.Primitive}[] ${in.Param.Name} = new ${in.Param.Type} {${foreach in.ValueList v ,}
					${v}${end}
				};
${<end}
${<end}
${<if !e.Output.Param.Type.Array}
				${e.Output.Param.Type.Primitive} __expected = ${e.Output};
${<else}
				${e.Output.Param.Type.Primitive}[] __expected = new ${e.Output.Param.Type} {${foreach e.Output.ValueList v ,}
					${v}${end}
				};
${<end}
				DoTest(${foreach e.Input in , }${in.Param.Name}${end}, __expected, cs);
				break;
			}
${<end}
			default: break;
		}
	}

	static void DoTest(${Method.Params}, ${Method.ReturnType} __expected, int caseNo) {
${<if RecordRuntime}
		DateTime startTime = DateTime.Now;
${<end}
		Exception exception = null;
		${ClassName} instance = new ${ClassName}();
		${Method.ReturnType} __result = ${Method.ReturnType;ZeroValue};
		try {
			__result = instance.${Method.Name}(${foreach Method.Params par , }${par.Name}${end});
		}
		catch (Exception e) { exception = e; }
${<if RecordRuntime}
		TimeSpan __elapsed = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
${<end}

		nAll++;
		Console.Error.Write(string.Format("  Testcase #{0} ... ", caseNo));

		if (exception != null) {
			Console.Error.WriteLine("RUNTIME ERROR!");
			Console.Error.WriteLine(exception);
		}
		else if (${if Method.ReturnType.Array}Equals(__result, __expected)${else}${if Method.ReturnType.RealNumber}DoubleEquals(__expected, __result)${else}__result == __expected${end}${end}) {
			Console.Error.WriteLine("PASSED! "${if RecordRuntime} + string.Format("({0:0.00} seconds)", __elapsed.TotalSeconds)${end});
			nPassed++;
		}
		else {
			Console.Error.WriteLine("FAILED! "${if RecordRuntime} + string.Format("({0:0.00} seconds)", __elapsed.TotalSeconds)${end});
			Console.Error.WriteLine("           Expected: " + ${if Method.ReturnType.Array}ToString(__expected)${else}__expected${end});
			Console.Error.WriteLine("           Received: " + ${if Method.ReturnType.Array}ToString(__result)${else}__result${end});
		}
	}

	static int nExample = ${NumOfExamples};
	static int nAll = 0, nPassed = 0;

${<if Method.ReturnType.RealNumber}
	static bool DoubleEquals(double a, double b) {
		return Math.Abs(a - b) < 1e-9 || Math.Abs(a) > Math.Abs(b) * (1.0 - 1e-9) && Math.Abs(a) < Math.Abs(b) * (1.0 + 1e-9);
	}

${<end}
${<if ReturnsArray}
	static bool Equals(${Method.ReturnType} a, ${Method.ReturnType} b) {
		if (a.Length != b.Length) return false;
		for (int i = 0; i < a.Length; ++i) if (${if Method.ReturnType.string}a[i] == null || b[i] == null || a[i] != b[i]${else}${if Method.ReturnType.RealNumber}!DoubleEquals(a[i], b[i])${else}a[i] != b[i]${end}${end}) return false;
		return true;
	}

	static string ToString(${Method.ReturnType} arr) {
		StringBuilder sb = new StringBuilder();
		sb.Append("[ ");
		for (int i = 0; i < arr.Length; ++i) {
			if (i > 0) sb.Append(", ");
			sb.Append(arr[i]);
		}
		return sb.ToString() + " ]";
	}
${<end}
	public static void Main(string[] args){
		Console.Error.WriteLine("${Problem.Name} (${Problem.Score} Points)");
		Console.Error.WriteLine();
		if (args.Length == 0)
			for (int i = 0; i < nExample; ++i) RunTestcase(i);
		else
			for (int i = 0; i < args.Length; ++i) RunTestcase(int.Parse(args[i]));
		Console.Error.WriteLine();
		Console.Error.WriteLine(string.Format("Passed : {0}/{1} cases", nPassed, nAll));

${<if RecordScore}
		DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		int T = (int)((DateTime.UtcNow - Jan1st1970).TotalSeconds - ${CreateTime});
		double PT = T / 60.0, TT = 75.0;
		Console.Error.WriteLine(string.Format("Time   : {0} minutes {1} secs", T / 60, T % 60));
		Console.Error.WriteLine(string.Format("Score  : {0:0.00} points", ${Problem.Score} * (0.3 + (0.7 * TT * TT) / (10.0 * PT * PT + TT * TT))));
${<end}
	}
