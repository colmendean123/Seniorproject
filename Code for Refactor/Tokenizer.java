package cs350f20project.controller.cli.parser;

import java.util.ArrayList;

import cs350f20project.controller.command.A_Command;
//The tokenizer class is the core of the parser.
public class Tokenizer {
	private ArrayList<String> tokens;
	int arg;
	private MyParserHelper parser;
	private int next;
	
	//seperate commandtext into an arraylist, removing any blanks, just like a real parser.
	public Tokenizer(String commandText, MyParserHelper helper) {
		arg = 0;
		this.next = 0;
		this.parser = helper;
		tokens = new ArrayList<String>();
		
		String[] tokengetter = commandText.split("\\s+");
		for(String token : tokengetter) {
			tokens.add(token.trim());
		}
	}
	
	// Returns the String contained in tokens that the index next is currently pointing to, then advances next 1
	public String getNext() {
		if(tokens.size() == 0)
			return null;
		
		int currentIndex = this.next;
		if(currentIndex >= this.tokens.size())
			return null;
		this.next++;
		return tokens.get(currentIndex);
	}
	
	public String get(int index) {
		if(index >= this.tokens.size())
			throw new RuntimeException("Error! Invalid token!");
		return tokens.get(index);
	}
	
	//get size of tokenizer. Length is size-1
	public int size() {
		return tokens.size();
	}
	
	public MyParserHelper getParser() {
		return parser;
	}
	
	//make it easy to call exception throws like invalid token. Return this at the bottom of every parse function.
	public A_Command invalidToken() throws RuntimeException{
			throw new RuntimeException("Error! invalid token! ");
		
	}
	
	public A_Command invalidToken(String message) throws RuntimeException{
		throw new RuntimeException(message);
	}
	
}
