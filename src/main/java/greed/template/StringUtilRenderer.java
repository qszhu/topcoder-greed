package greed.template;

import com.floreysoft.jmte.NamedRenderer;
import com.floreysoft.jmte.RenderFormatInfo;

import java.util.Locale;

/**
 * Greed is good! Cheers!
 */
public class StringUtilRenderer implements NamedRenderer {
    @Override
    public String render(Object o, String param, Locale locale) {
        if (o instanceof String) {
            String result = (String) o;
            for (String func: param.split(",")) {
                if (func.trim().equals("lower")) {
                    result = result.toLowerCase();
                }
                else if (func.trim().equals("upfirst")) {
                    if (result.length() > 0)
                        result = result.substring(0, 1).toUpperCase() + result.substring(1).toLowerCase();
                }
                else if (func.trim().equals("removespace")) {
                    result = result.replaceAll("\\s", "");
                }
                else if (func.trim().equals("unquote")) {
                    if (result.length() >= 2 && result.charAt(0) == '"' && result.charAt(result.length() - 1) == '"')
                        result = result.substring(1, result.length() - 1);
                }
                else if (func.trim().equals("abbr")) {
                    String[] tokens = result.split("\\s+");
                    StringBuilder abbr = new StringBuilder();
                    for (String tok: tokens) {
                        if (allDigits(tok) || allUppercase(tok))
                            abbr.append(tok);
                        else
                            abbr.append(tok.substring(0, 1).toUpperCase());
                    }
                    result = abbr.toString();
                }
            }
            return result;
        }
        return "";
    }

    @Override
    public String getName() {
        return "string";
    }

    private boolean allDigits(String s) {
        for (int i = 0; i < s.length(); ++i)
            if (!Character.isDigit(s.charAt(i)))
                return false;
        return true;
    }

    private boolean allUppercase(String s) {
        for (int i = 0; i < s.length(); ++i)
            if (!Character.isUpperCase(s.charAt(i)))
                return false;
        return true;
    }

    @Override
    public RenderFormatInfo getFormatInfo() {
        return null;
    }

    @Override
    public Class<?>[] getSupportedClasses() {
        return new Class<?>[]{String.class};
    }
}
