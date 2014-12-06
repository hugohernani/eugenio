/// <summary>
/// Main menu.
/// Attached to Main Camera
/// </summary>

using UnityEngine;
using System.Collections;

public class MainMenuBackup : MonoBehaviour {
	private float originalWidth = 1024.0f;
	private float originalHeight = 768.0f;

	// Quantity of Dimensions
	private const int QTY_DIM = 2;

	// Quantity of Buttons in Menu Inf
	private const int QTY_BUTTON_MENU_INF = 4;
	private const int QTY_BUTTONS_MENU_DES = 5;

	// Multiplication Factors
	private readonly float[] FM_MENU_INF = new float[2] {0.76f, 0.24f};
	private readonly float[] FM_MENU_DES = new float[2] {0.92f, 0.2f};
	private readonly float[] FM_EUGENIO = new float[2] {0.36f, 0.5f};
	private readonly float[] FM_BUTTON_DOWN = new float[2] {0.17f, 0.69f};
	private readonly float[] FM_BUTTON_UP = new float[2] {0.12f, 0.71f};

	// Positions
	private readonly float[] POS_MENU_INF;
	private readonly float[] POS_MENU_DES;
	private readonly float[] POS_EUGENIO;
	private readonly float POS_BUTTONS_DOWN_Y;
	private readonly float[] POS_BUTTONS_DOWN_X;
	private readonly float POS_BUTTONS_UP_Y;
	private readonly float[] POS_BUTTONS_UP_X;

	// Size
	private readonly float[] SIZE_MENU_INF;
	private readonly float[] SIZE_MENU_DES;
	private readonly float[] SIZE_EUGENIO;
	private readonly float[] SIZE_BUTTONS_DOWN;
	private readonly float[] SIZE_BUTTONS_UP;

	// Textures
	public Texture backgroundTexture;
	public Texture eugenioTexture;
	public Texture menuInfTexture;
	public Texture menuDesTexture;

	// Buttons
	public GUIStyle[] buttonsInf = new GUIStyle[QTY_BUTTON_MENU_INF];
	public GUIStyle[] buttonsDes = new GUIStyle[QTY_BUTTONS_MENU_DES];

	// Use this for initialization
	public MainMenuBackup () {
		Init (out POS_MENU_INF, out POS_MENU_DES, out POS_EUGENIO, 
		      out POS_BUTTONS_DOWN_Y, out POS_BUTTONS_DOWN_X,
		      out POS_BUTTONS_UP_Y, out POS_BUTTONS_UP_X,
		      out SIZE_MENU_INF, out SIZE_MENU_DES, out SIZE_EUGENIO,
		      out SIZE_BUTTONS_DOWN, out SIZE_BUTTONS_UP);
	}

	protected virtual void Init(out float[] POS_MENU_INF, out float[] POS_MENU_DES, out float[] POS_EUGENIO,
	                            out float POS_BUTTONS_DOWN_Y, out float[] POS_BUTTONS_DOWN_X,
	                            out float POS_BUTTONS_UP_Y, out float[] POS_BUTTONS_UP_X,
	                            out float[] SIZE_MENU_INF, out float[] SIZE_MENU_DES, out float[] SIZE_EUGENIO,
	                            out float[] SIZE_BUTTONS_DOWN, out float[] SIZE_BUTTONS_UP){

		// Initializing variables
		POS_BUTTONS_DOWN_X = new float[QTY_BUTTON_MENU_INF];
		POS_BUTTONS_UP_X = new float[QTY_BUTTONS_MENU_DES];
		POS_MENU_INF = new float[QTY_DIM];
		POS_MENU_DES = new float[QTY_DIM];
		POS_EUGENIO = new float[QTY_DIM];
		SIZE_MENU_INF = new float[QTY_DIM];
		SIZE_MENU_DES = new float[QTY_DIM];
		SIZE_EUGENIO = new float[QTY_DIM];
		SIZE_BUTTONS_DOWN = new float[QTY_DIM];
		SIZE_BUTTONS_UP = new float[QTY_DIM];

		/* Menu Inf */
		// Set size
		SIZE_MENU_INF[0] = Screen.width * FM_MENU_INF[0];
		SIZE_MENU_INF[1] = Screen.height * FM_MENU_INF[1];
		// Set positions
		POS_MENU_INF[0] = (Screen.width - SIZE_MENU_INF[0]) / 2;
		POS_MENU_INF[1] = Screen.height - SIZE_MENU_INF[1];

		/* Menu Des */
		// Set Size
		SIZE_MENU_DES [0] = Screen.width * FM_MENU_DES [0];
		SIZE_MENU_DES [1] = Screen.height * FM_MENU_DES [1];
		// Set positions
		POS_MENU_DES [0] = (Screen.width - SIZE_MENU_DES [0]) / 2;
		POS_MENU_DES [1] = 0;

		/* Eugenio */
		// Set size
		SIZE_EUGENIO [0] = Screen.width * FM_EUGENIO [0];
		SIZE_EUGENIO [1] = Screen.height * FM_EUGENIO [1];
		// Set positions
		POS_EUGENIO [0] = (Screen.width - SIZE_EUGENIO [0]) / 2;
		POS_EUGENIO [1] = (Screen.height - SIZE_EUGENIO [1]) / 2;

		/* Down Buttons */
		// Set size
		SIZE_BUTTONS_DOWN[0] = SIZE_MENU_INF[0] * FM_BUTTON_DOWN[0];
		SIZE_BUTTONS_DOWN[1] = SIZE_MENU_INF[1] * FM_BUTTON_DOWN[1];
		// Set position Y
		POS_BUTTONS_DOWN_Y = POS_MENU_INF[1] + (SIZE_MENU_INF[1] - SIZE_BUTTONS_DOWN[1]) / 2;

		/* Up Buttons */
		// Set size
		SIZE_BUTTONS_UP [0] = SIZE_MENU_DES[0] * FM_BUTTON_UP [0];
		SIZE_BUTTONS_UP [1] = SIZE_MENU_DES [1] * FM_BUTTON_UP [1];
		// Set position Y
		POS_BUTTONS_UP_Y = POS_MENU_DES [1] + (SIZE_MENU_DES [1] - SIZE_BUTTONS_UP [1])/4;

		/* Calc space between buttons */
		float SPACE_BUTTONS_DOWN_X = (SIZE_MENU_INF[0] - QTY_BUTTON_MENU_INF * SIZE_BUTTONS_DOWN[0]) / (QTY_BUTTON_MENU_INF + 1);
		float SPACE_BUTTONS_UP_X = (SIZE_MENU_DES [0] - QTY_BUTTONS_MENU_DES * SIZE_BUTTONS_UP [0]) / (QTY_BUTTONS_MENU_DES + 1);

		// Set positions Buttons X
		// Down
		for(int i = 0; i < QTY_BUTTON_MENU_INF; i++){
			POS_BUTTONS_DOWN_X[i] = POS_MENU_INF[0] + (i+1)*SPACE_BUTTONS_DOWN_X + i*SIZE_BUTTONS_DOWN[0]; 
		}

		// UP
		for(int i = 0; i < QTY_BUTTONS_MENU_DES; i++){
			POS_BUTTONS_UP_X[i] = POS_MENU_DES[0] + (i+1)*SPACE_BUTTONS_UP_X + i*SIZE_BUTTONS_UP[0]; 
		}
	}

	void OnGUI(){
		/*
		setMatrix ();

		// Display Background Texture
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);

		// Display Eugenio Texture
		GUI.DrawTexture (new Rect (POS_EUGENIO[0], POS_EUGENIO[1], SIZE_EUGENIO[0], SIZE_EUGENIO[1]), 
		                 eugenioTexture);

		// Display Menu Inf Texture
		GUI.DrawTexture (new Rect (POS_MENU_INF[0], POS_MENU_INF[1], SIZE_MENU_INF[0], 
		                           SIZE_MENU_INF[1]), menuInfTexture);

		// Display Menu Des Texture
		GUI.DrawTexture (new Rect (POS_MENU_DES[0], POS_MENU_DES[1], SIZE_MENU_DES[0],
		                           SIZE_MENU_DES[1]), menuDesTexture);


		// Display Buttons Inf
		for (int i = 0; i < QTY_BUTTON_MENU_INF; i++) {
			if (GUI.Button (new Rect (POS_BUTTONS_DOWN_X[i], POS_BUTTONS_DOWN_Y, SIZE_BUTTONS_DOWN[0], 
			                          SIZE_BUTTONS_DOWN[1]), "", buttonsInf[i])) {
				Application.LoadLevel("Estudar");
			}
		}

		// Display Buttons Des
		for (int i = 0; i < QTY_BUTTONS_MENU_DES; i++) {
			if (GUI.Button (new Rect (POS_BUTTONS_UP_X [i], POS_BUTTONS_UP_Y, SIZE_BUTTONS_UP [0], 
                  SIZE_BUTTONS_UP [1]), "", buttonsDes[i])) {
					
				print ("clicado no botao des");
			}
		}

		resetMatrix ();
		*/
	}

	private void setMatrix(){
		Vector2 ratio = new Vector2(Screen.width/originalWidth , Screen.height/originalHeight );
		Matrix4x4 guiMatrix = Matrix4x4.identity;
		guiMatrix.SetTRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(ratio.x, ratio.y, 1));
		GUI.matrix = guiMatrix;
	}
	
	private void resetMatrix(){
		
		GUI.matrix = Matrix4x4.identity;
	}
}
