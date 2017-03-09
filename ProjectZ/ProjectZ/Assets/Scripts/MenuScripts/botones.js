public function BotonJugar(){
	
	Application.LoadLevel("ProbandoQuads");
	
}

public function BotonMenu(){
	
    Debug.Log("Change");
    Application.LoadLevel("AlphaMenu");
	
}

public function BotonSalir(){
	
	Application.Quit();
	
}