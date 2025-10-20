grammar LabCalculator;

/*
* Parser Rules
*/
compileUnit : expression EOF;
expression :
    LPAREN expression RPAREN #ParenthesizedExpr
    | SUBTRACT expression #UnaryMinusExpr
    | ADD expression #UnaryPlusExpr
    | expression EXPONENT expression #ExponentialExpr
    | expression operatorToken=(MULTIPLY | DIVIDE | MOD | DIV) expression #MultiplicativeExpr
    | expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
    | expression operatorToken=(EQUAL | LESS | GREATER | LESSEQUAL | GREATEREQUAL | NOTEQUAL) expression #ComparisonExpr
    | NUMBER #NumberExpr
    | IDENTIFIER #IdentifierExpr
    ;
/*
* Lexer Rules
*/
NUMBER       : INT ('.' INT)?;
IDENTIFIER   : [a-zA-Z]+[1-9][0-9]*; 
INT          : [0-9]+;
EXPONENT     : '^';
MULTIPLY     : '*';
DIVIDE       : '/';
MOD          : 'mod';
DIV          : 'div';
SUBTRACT     : '-';
ADD          : '+';
EQUAL        : '=';
LESS         : '<';
GREATER      : '>';
LESSEQUAL    : '<=';
GREATEREQUAL : '>=';
NOTEQUAL     : '<>';
LPAREN       : '(';
RPAREN       : ')';
WS           : [ \t\r\n] -> channel(HIDDEN);