package com.matsn.toturial.struts;

public class HelloAction {
	
	private  String name;
	
	public String execute() throws Exception{
		return "success";
	}
	
	public String getName(){
		return this.name;
	}

	public void setName(String name){
		this.name = name;
	}
}
