// Generated from d:/Lab1/Pexel/Parser/LabCalculator.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class LabCalculatorLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		NUMBER=1, IDENTIFIER=2, INT=3, EXPONENT=4, MULTIPLY=5, DIVIDE=6, MOD=7, 
		DIV=8, SUBTRACT=9, ADD=10, EQUAL=11, LESS=12, GREATER=13, LESSEQUAL=14, 
		GREATEREQUAL=15, NOTEQUAL=16, LPAREN=17, RPAREN=18, WS=19;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"NUMBER", "IDENTIFIER", "INT", "EXPONENT", "MULTIPLY", "DIVIDE", "MOD", 
			"DIV", "SUBTRACT", "ADD", "EQUAL", "LESS", "GREATER", "LESSEQUAL", "GREATEREQUAL", 
			"NOTEQUAL", "LPAREN", "RPAREN", "WS"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, "'^'", "'*'", "'/'", "'mod'", "'div'", "'-'", 
			"'+'", "'='", "'<'", "'>'", "'<='", "'>='", "'<>'", "'('", "')'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "NUMBER", "IDENTIFIER", "INT", "EXPONENT", "MULTIPLY", "DIVIDE", 
			"MOD", "DIV", "SUBTRACT", "ADD", "EQUAL", "LESS", "GREATER", "LESSEQUAL", 
			"GREATEREQUAL", "NOTEQUAL", "LPAREN", "RPAREN", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public LabCalculatorLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "LabCalculator.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000\u0013f\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002\u0001"+
		"\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004"+
		"\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007"+
		"\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b"+
		"\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002"+
		"\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002"+
		"\u0012\u0007\u0012\u0001\u0000\u0001\u0000\u0001\u0000\u0003\u0000+\b"+
		"\u0000\u0001\u0001\u0004\u0001.\b\u0001\u000b\u0001\f\u0001/\u0001\u0001"+
		"\u0001\u0001\u0005\u00014\b\u0001\n\u0001\f\u00017\t\u0001\u0001\u0002"+
		"\u0004\u0002:\b\u0002\u000b\u0002\f\u0002;\u0001\u0003\u0001\u0003\u0001"+
		"\u0004\u0001\u0004\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006\u0001"+
		"\u0006\u0001\u0006\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001"+
		"\b\u0001\b\u0001\t\u0001\t\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0001"+
		"\f\u0001\f\u0001\r\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000e"+
		"\u0001\u000f\u0001\u000f\u0001\u000f\u0001\u0010\u0001\u0010\u0001\u0011"+
		"\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0012\u0001\u0012\u0000\u0000"+
		"\u0013\u0001\u0001\u0003\u0002\u0005\u0003\u0007\u0004\t\u0005\u000b\u0006"+
		"\r\u0007\u000f\b\u0011\t\u0013\n\u0015\u000b\u0017\f\u0019\r\u001b\u000e"+
		"\u001d\u000f\u001f\u0010!\u0011#\u0012%\u0013\u0001\u0000\u0004\u0002"+
		"\u0000AZaz\u0001\u000019\u0001\u000009\u0003\u0000\t\n\r\r  i\u0000\u0001"+
		"\u0001\u0000\u0000\u0000\u0000\u0003\u0001\u0000\u0000\u0000\u0000\u0005"+
		"\u0001\u0000\u0000\u0000\u0000\u0007\u0001\u0000\u0000\u0000\u0000\t\u0001"+
		"\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000\u0000\u0000\r\u0001\u0000"+
		"\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000\u0000\u0011\u0001\u0000"+
		"\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000\u0000\u0015\u0001\u0000"+
		"\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000\u0000\u0019\u0001\u0000"+
		"\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000\u0000\u001d\u0001\u0000"+
		"\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000\u0000!\u0001\u0000\u0000"+
		"\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%\u0001\u0000\u0000\u0000\u0001"+
		"\'\u0001\u0000\u0000\u0000\u0003-\u0001\u0000\u0000\u0000\u00059\u0001"+
		"\u0000\u0000\u0000\u0007=\u0001\u0000\u0000\u0000\t?\u0001\u0000\u0000"+
		"\u0000\u000bA\u0001\u0000\u0000\u0000\rC\u0001\u0000\u0000\u0000\u000f"+
		"G\u0001\u0000\u0000\u0000\u0011K\u0001\u0000\u0000\u0000\u0013M\u0001"+
		"\u0000\u0000\u0000\u0015O\u0001\u0000\u0000\u0000\u0017Q\u0001\u0000\u0000"+
		"\u0000\u0019S\u0001\u0000\u0000\u0000\u001bU\u0001\u0000\u0000\u0000\u001d"+
		"X\u0001\u0000\u0000\u0000\u001f[\u0001\u0000\u0000\u0000!^\u0001\u0000"+
		"\u0000\u0000#`\u0001\u0000\u0000\u0000%b\u0001\u0000\u0000\u0000\'*\u0003"+
		"\u0005\u0002\u0000()\u0005.\u0000\u0000)+\u0003\u0005\u0002\u0000*(\u0001"+
		"\u0000\u0000\u0000*+\u0001\u0000\u0000\u0000+\u0002\u0001\u0000\u0000"+
		"\u0000,.\u0007\u0000\u0000\u0000-,\u0001\u0000\u0000\u0000./\u0001\u0000"+
		"\u0000\u0000/-\u0001\u0000\u0000\u0000/0\u0001\u0000\u0000\u000001\u0001"+
		"\u0000\u0000\u000015\u0007\u0001\u0000\u000024\u0007\u0002\u0000\u0000"+
		"32\u0001\u0000\u0000\u000047\u0001\u0000\u0000\u000053\u0001\u0000\u0000"+
		"\u000056\u0001\u0000\u0000\u00006\u0004\u0001\u0000\u0000\u000075\u0001"+
		"\u0000\u0000\u00008:\u0007\u0002\u0000\u000098\u0001\u0000\u0000\u0000"+
		":;\u0001\u0000\u0000\u0000;9\u0001\u0000\u0000\u0000;<\u0001\u0000\u0000"+
		"\u0000<\u0006\u0001\u0000\u0000\u0000=>\u0005^\u0000\u0000>\b\u0001\u0000"+
		"\u0000\u0000?@\u0005*\u0000\u0000@\n\u0001\u0000\u0000\u0000AB\u0005/"+
		"\u0000\u0000B\f\u0001\u0000\u0000\u0000CD\u0005m\u0000\u0000DE\u0005o"+
		"\u0000\u0000EF\u0005d\u0000\u0000F\u000e\u0001\u0000\u0000\u0000GH\u0005"+
		"d\u0000\u0000HI\u0005i\u0000\u0000IJ\u0005v\u0000\u0000J\u0010\u0001\u0000"+
		"\u0000\u0000KL\u0005-\u0000\u0000L\u0012\u0001\u0000\u0000\u0000MN\u0005"+
		"+\u0000\u0000N\u0014\u0001\u0000\u0000\u0000OP\u0005=\u0000\u0000P\u0016"+
		"\u0001\u0000\u0000\u0000QR\u0005<\u0000\u0000R\u0018\u0001\u0000\u0000"+
		"\u0000ST\u0005>\u0000\u0000T\u001a\u0001\u0000\u0000\u0000UV\u0005<\u0000"+
		"\u0000VW\u0005=\u0000\u0000W\u001c\u0001\u0000\u0000\u0000XY\u0005>\u0000"+
		"\u0000YZ\u0005=\u0000\u0000Z\u001e\u0001\u0000\u0000\u0000[\\\u0005<\u0000"+
		"\u0000\\]\u0005>\u0000\u0000] \u0001\u0000\u0000\u0000^_\u0005(\u0000"+
		"\u0000_\"\u0001\u0000\u0000\u0000`a\u0005)\u0000\u0000a$\u0001\u0000\u0000"+
		"\u0000bc\u0007\u0003\u0000\u0000cd\u0001\u0000\u0000\u0000de\u0006\u0012"+
		"\u0000\u0000e&\u0001\u0000\u0000\u0000\u0005\u0000*/5;\u0001\u0000\u0001"+
		"\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}