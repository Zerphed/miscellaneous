import java.Math;
import java.util.ArrayList;

//////////// PRELOADED ASSETS ///////////

/* @pjs preload="images/starfield_background.jpg,sprites/spaceship_0.png,sprites/asteroid_0.png,
                 sprites/asteroid_1.png,sprites/asteroid_2.png,sprites/asteroid_3.png,
                 sprites/asteroid_4.png,sprites/asteroid_5.png,sprites/asteroid_6.png,sprites/asteroid_7.png,
                 sprites/spaceship_accelerating_0.png,sprites/spaceship_accelerating_1.png,
                 sprites/spaceship_turning_r_0.png, sprites/spaceship_turning_l_0.png,
                 sprites/spaceship_accelerating_turning_r_0.png,sprites/spaceship_accelerating_turning_r_1.png,
                 sprites/spaceship_accelerating_turning_l_0.png,sprites/spaceship_accelerating_turning_l_1.png,
                 sprites/asteroid_explosion_0.png,sprites/asteroid_explosion_1.png,sprites/asteroid_explosion_2.png,
                 sprites/asteroid_explosion_3.png,sprites/asteroid_explosion_4.png,sprites/asteroid_explosion_5.png,
                 sprites/asteroid_explosion_6.png,sprites/asteroid_explosion_7.png,sprites/asteroid_explosion_8.png,
                 sprites/asteroid_explosion_9.png,sprites/asteroid_explosion_9.png,sprites/asteroid_explosion_10.png,
                 sprites/asteroid_explosion_11.png,sprites/asteroid_explosion_12.png,sprites/asteroid_explosion_13.png,
                 sprites/asteroid_explosion_14.png,sprites/asteroid_explosion_15.png,sprites/asteroid_explosion_16.png,
                 sprites/asteroid_explosion_17.png,sprites/asteroid_explosion_18.png,sprites/asteroid_explosion_19.png,
                 sprites/asteroid_explosion_20.png,sprites/asteroid_explosion_21.png,sprites/asteroid_explosion_22.png,
                 sprites/asteroid_explosion_23.png,sprites/asteroid_explosion_24.png,sprites/asteroid_explosion_25.png,
                 sprites/asteroid_explosion_26.png,
                 sprites/player_explode_1.png,sprites/player_explode_2.png,sprites/player_explode_2.png,sprites/player_explode_3.png,
                 sprites/player_explode_4.png,sprites/player_explode_5.png,sprites/player_explode_6.png,sprites/player_explode_7.png,
                 sprites/player_explode_8.png,sprites/player_explode_9.png,sprites/player_explode_10.png,sprites/player_explode_11.png,
                 sprites/player_explode_12.png,sprites/player_explode_13.png,sprites/player_explode_14.png,sprites/player_explode_15.png,
                 sprites/player_explode_16.png,sprites/player_explode_17.png,sprites/player_explode_18.png,sprites/player_explode_19.png,
                 sprites/player_explode_20.png,sprites/player_explode_21.png,sprites/player_explode_22.png,sprites/player_explode_23.png,
                 sprites/player_explode_24.png,sprites/player_explode_25.png,sprites/player_explode_26.png,sprites/player_explode_27.png,
                 sprites/player_explode_28.png,sprites/player_explode_29.png,sprites/player_explode_30.png,sprites/player_explode_31.png,
                 sprites/player_explode_32.png,sprites/player_explode_33.png,sprites/player_explode_34.png,sprites/player_explode_35.png,
                 sprites/player_explode_36.png,sprites/player_explode_37.png,sprites/player_explode_38.png,sprites/player_explode_39.png,
                 sprites/player_explode_40.png,sprites/player_explode_41.png,sprites/player_explode_42.png"; 

        font="fonts/helveticaneue-webfont.ttf";
*/ 


var soundEffects = {
    // Background sounds
    "background_music"        : AudioFX('audio/background_1', { formats: ['mp3'], autoplay: true, loop: true }),
    // Player sound effects
    "shoot_effect"            : AudioFX('audio/shoot_effect',   { formats: ['wav','mp3'], pool: 15 }),
    "accelerate_effect"       : AudioFX('audio/accelerate_effect', { formats: ['mp3'], loop: true, pool: 2 }),
    "player_explode_effect"   : AudioFX('audio/player_explode_effect', { formats: ['mp3'] }),
    // Asteroid sound effects
    "asteroid_explode_effect" : AudioFX('audio/asteroid_explode_effect',   { formats: ['mp3'], pool: 15 })
};


///////////// DEBUG ////////////////

public class Debug {
    public static void drawCircumcircle(double r) {
        stroke(255, 0, 0);
        noFill();
        ellipse(0, 0, r, r);
    }
}

///////////// UTILITIES /////////////////

public class Utilities {
    
    public static int randomInt(int min, int max) {
        return min + (int)(Math.random() * ((max - min) + 1));
    }
    
    public static double randomDouble(double min, double max) {
        return Math.random() * max;
    }
    
    public static double calculateCircumradius(double width, double height) {
        return Math.sqrt(width*width + height*height)/2.0;
    }
}


///////////// SPRITE ////////////////////

public class Sprite {
	
	private PImage[] images;
	
    private int imageCount;
	private int frame;
	private int updateCooldown;
    private int updateCounter;
    
    private int width;
    private int height;
    
    Sprite(String imagePrefix, int count, int duration) {
        this.imageCount = count;
        this.frame = 0;
        
		this.images = new PImage[count];
        this.updateCooldown = int(FRAME_RATE * duration / float(count));
        this.updateCounter = 0;
        
		for (int i = 0; i < count; i++) {
			String filename = "sprites/" + imagePrefix + "_" + i + ".png";
			this.images[i] = requestImage(filename);
		}
        
        this.width = this.images[0].width;
        this.height = this.images[0].height;
    }
    
	Sprite(String imagePrefix, int count) {
		this.imageCount = count;
        this.frame = Utilities.randomInt(0, count-1);
        
		this.images = new PImage[count];
        this.updateCooldown = SPRITE_COOLDOWN;
        this.updateCounter = 0;
        
		for (int i = 0; i < count; i++) {
			String filename = "sprites/" + imagePrefix + "_" + i + ".png";
			this.images[i] = requestImage(filename);
		}
        
        this.width = this.images[0].width;
        this.height = this.images[0].height;
	}
	
	void draw() {
        if (this.updateCounter == 0) {
            this.frame = (this.frame+1)%this.imageCount;
            this.updateCounter = this.updateCooldown;
        }
        else {
            updateCounter--;
        }
            
        imageMode(CENTER);
		image(this.images[frame], 0, 0);
	}
}

public class Animation {
    private int x;
    private int y;
    private int endTime;
    private Sprite sprite;
    
    Animation (String spriteImagePrefix, int numOfImages, int x, int y, String soundHandle) {
        
        this.x = x;
        this.y = y;
        
        var tempHandle = soundEffects[soundHandle].audio[0] || soundEffects[soundHandle].audio;
        
        this.endTime = int(millis() + tempHandle.duration*1000);
        this.sprite = new Sprite(spriteImagePrefix, numOfImages, tempHandle.duration);
        soundEffects[soundHandle].play();
    }
    
    void draw() {
       pushMatrix();
            translate(this.x, this.y);
            this.sprite.draw();
        popMatrix();
    }
    
    boolean isFinished() {
        if (this.endTime < millis())
            return true;
        return false;
    }
}


//////////// DRAWABLE OBJECT /////////////

public abstract class DrawableObject {

    private int x;          // X coordinate on the canvas
    private int y;          // Y coordinate on the canvas
    
    private double vx;      // Velocity on the x-axis
    private double vy;      // Velocity on the y-axis
    
    private double r;       // Radius of the bounding circle or circle

    private Sprite sprite;  // Default sprite of the object
    
    public boolean collide(DrawableObject other) {
        double dx = other.x - this.x;
        double dy = other.y - this.y;
        
        double distance = Math.sqrt(dx*dx + dy*dy);
        
        double collisionDist = other.r + this.r;
        if (distance < collisionDist) {
            return true;
        }
        return false;
    }
    
    public void update() {
    	this.move();
    	this.draw();
    }
    
    abstract void draw();
    
    public void move() {
        this.x += vx; 
        this.y += vy;
        
        if (this.x > w.getWidth() + this.r*2) {
        	this.x = 0;
        }
        if (this.y  > w.getHeight() + this.r*2) {
        	this.y = 0;
        }
        
        if (this.y + this.r*2 < 0) {
        	this.y = w.getHeight();
        }
        if (this.x + this.r*2 < 0) {
        	this.x = w.getWidth();
        }
        
    }
    
}

///////////////// PLAYER /////////////////////

public class Player extends DrawableObject {

    // Sprite definitions
    static final String normalSpriteImg = "spaceship";
    static final int normalSpriteImgCount = 1;
    private Sprite normalSprite;
    
    static final String normalTRSpriteImg = "spaceship_turning_r";
    static final int normalTRSpriteImgCount = 1;
    private Sprite normalTRSprite;
    
    static final String normalTLSpriteImg = "spaceship_turning_l";
    static final int normalTLSpriteImgCount = 1;
    private Sprite normalTLSprite;
    
    static final String acceleratingSpriteImg = "spaceship_accelerating";
    static final String acceleratingSpriteImgCount = 2;
    private Sprite acceleratingSprite;
    
    static final String acceleratingTRSpriteImg = "spaceship_accelerating_turning_r";
    static final String acceleratingTRSpriteImgCount = 2;
    private Sprite acceleratingTRSprite;
    
    static final String acceleratingTLSpriteImg = "spaceship_accelerating_turning_l";
    static final String acceleratingTLSpriteImgCount = 2;
    private Sprite acceleratingTLSprite;
    
    // Death animation definitions
    static final String dieSpriteImg = "player_explode";
    static final int dieSpriteImgCount = 43;
    static final String dieSoundFx = "player_explode_effect";
    
    // State variables
    private double angle;
    private int missileCooldown;
    private int score;
    
    private boolean isAccelerating;
    private boolean isTurningRight;
    private boolean isTurningLeft;
    
    public Player(int x, int y) {
        this.x = x;
        this.y = y;
        
        this.vx = 0.0;
        this.vy = 0.0;
        
        this.normalSprite = new Sprite(this.normalSpriteImg, this.normalSpriteImgCount);
        this.normalTRSprite = new Sprite(this.normalTRSpriteImg, this.normalTRSpriteImgCount);
        this.normalTLSprite = new Sprite(this.normalTLSpriteImg, this.normalTLSpriteImgCount);
        
        this.acceleratingSprite = new Sprite(this.acceleratingSpriteImg, this.acceleratingSpriteImgCount);
        this.acceleratingTRSprite = new Sprite(this.acceleratingTRSpriteImg, this.acceleratingTRSpriteImgCount);
        this.acceleratingTLSprite = new Sprite(this.acceleratingTLSpriteImg, this.acceleratingTLSpriteImgCount);
        
        this.sprite = normalSprite;
        
        this.r = Utilities.calculateCircumradius(this.sprite.width, this.sprite.height);
        
        this.angle = 0.0;
        this.missileCooldown = 0;
        this.score = 0;
        
        this.isAccelerating = this.isTurningRight = this.isTurningLeft = false;        
    }
    
    public void draw() {
        pushMatrix();
            translate(this.x, this.y);
            rotate(angle);
            this.sprite.draw();
            if (DEBUG) {
                Debug.drawCircumcircle(this.r);
            }
        popMatrix();
    }
    
    public void update() {
        super.update();
        if (this.missileCooldown > 0)
            this.missileCooldown--;
        
        this.vx *= DAMPENING;
        this.vy *= DAMPENING;
    }
    
    public Animation die() {
        return new Animation(this.dieSpriteImg, this.dieSpriteImgCount, this.x, this.y, this.dieSoundFx);
    }
    
    public double getTotalVelocity() {
        return Math.sqrt(this.vx*this.vx + this.vy*this.vy);
    }
    
    public void shoot() {
        if (this.missileCooldown <= 0) {
            soundEffects["shoot_effect"].play();
        
            double cosA = Math.cos(angle);
            double sinA = Math.sin(angle);
            
            Missile m = new Missile(this.x + this.r * cosA, this.y + this.r * sinA, MISSILE_VELOCITY * cosA, 
                MISSILE_VELOCITY * sinA, this.angle);
            w.addMissile(m);
            this.missileCooldown = MISSILE_COOLDOWN;
        }
    }

    public void addScore(int num) {
        this.score += num;
    }
    
    public void removeScore(int num) {
        this.score -= num;
    }
    
    public int getScore() {
        return this.score;
    }
    
    private void toggleSprite() {
        if (this.isAccelerating) {
            if (this.isTurningRight)
                this.sprite = this.acceleratingTRSprite;
            else if (this.isTurningLeft)
                this.sprite = this.acceleratingTLSprite;
            else
                this.sprite = this.acceleratingSprite;
        }
        else {
            if (this.isTurningRight)
                this.sprite = this.normalTRSprite;
            else if (this.isTurningLeft)
                this.sprite = this.normalTLSprite;
            else
                this.sprite = this.normalSprite;
        }
    }
    
    // Set accelerating state
    public void setAccelerating() {
        if (!this.isAccelerating) {
            this.isAccelerating = true;
            soundEffects["accelerate_effect"].play();
            this.toggleSprite();
        }
    }
    
    // Unset accelerating state
    public void unsetAccelerating() {
        if (this.isAccelerating) {
            this.isAccelerating = false;
            soundEffects["accelerate_effect"].stop();
            this.toggleSprite();
        }
    }
    
    // Set turning right state
    public void setTurningRight() {
        if (!this.isTurningRight) {
            this.isTurningRight = true;
            this.toggleSprite();
        }
    }
    
    // Unset turning right state
    public void unsetTurningRight() {
        if (this.isTurningRight) {
            this.isTurningRight = false;
            this.toggleSprite();
        }
    }
    
    // Set turning left state
    public void setTurningLeft() {
        if (!this.isTurningLeft) {
            this.isTurningLeft = true;
            this.toggleSprite();
        }
    }
    
    // Unset turning left state
    public void unsetTurningLeft() {
        if (this.isTurningLeft) {
            this.isTurningLeft = false;
            this.toggleSprite();
        }
    }
}

///////////////// ASTEROID /////////////////////

public class Asteroid extends DrawableObject {
	
    static final String normalSpriteImg = "asteroid";
    static final int normalSpriteImgCount = 8;
    
    static final String dieSpriteImg = "asteroid_explosion";
    static final int dieSpriteImgCount = 26;
    static final String dieSoundFx = "asteroid_explode_effect";
    
	public Asteroid (int x, int y, double vx, double vy) {
        this.x = x;
        this.y = y;
        
        this.vx = vx;
        this.vy = vy;
        
        this.sprite = new Sprite(this.normalSpriteImg, this.normalSpriteImgCount);
        this.r = Utilities.calculateCircumradius(this.sprite.width, this.sprite.height);
	}
	
    public Animation die() {
        return new Animation(this.dieSpriteImg, this.dieSpriteImgCount, this.x, this.y, this.dieSoundFx)
    }
    
    public void draw() {
        pushMatrix();
            translate(this.x, this.y);
            
            this.sprite.draw();
            
            if (DEBUG) {
                Debug.drawCircumcircle(this.r);
            }
        popMatrix();
    }

	
}

///////////////// MISSILE /////////////////////

public class Missile extends DrawableObject {
	
	private double angle;
	private int ttl; // TIME TO LIVE
	
	public Missile(int x, int y, double vx, double vy, double angle){
		
        this.x = x;
		this.y = y;
        
        this.vx = vx;
		this.vy = vy;
        
		this.r = 5;
		
        this.angle = angle;
		this.ttl = MISSILE_TIME_TO_LIVE;
	}
	
    public void draw() {
        stroke(255, 0, 0);
        fill(255, 255, 255);
        ellipse(this.x, this.y, this.r, this.r);
    }
    
    public void update() {
    	super.update();
    	this.ttl--;
    }
}

///////////////// WORLD /////////////////////

public class World {
	
	private int width;
	private int height;
    
    private ProceduralBackground bg;
    
    private int backgroundOffsetX;
    
	// How many asteroids in the beginning of the world
	private int numberOfAsteroids;
	private int currentWave;
    private int nextWaveComing;
	private Player player;
	
    private ArrayList asteroids = new ArrayList();
	private ArrayList missiles = new ArrayList();
    private ArrayList scheduler = new ArrayList();
    
    private PImage backgroundImage;
    
    private boolean gameOver = false;
    
	World(int width, int height, int numOfAsteroids) {
        
		this.width = width;
		this.height = height;
        
        this.bg = new ProceduralBackground(this.width, this.height);
        
		this.currentWave = 0;
        this.nextWaveComing = 0;
		this.player = new Player(width/2.0, height/2.0);
        
        this.backgroundOffsetX = 0;
		this.backgroundImage = loadImage(BACKGROUND_IMAGE);        
        this.backgroundImage.resize(this.width, this.height);
	}
	
	public void addAsteroids(int totalAsteroids){
		
		this.numberOfAsteroids = totalAsteroids;
		for(int i = 0; i < numberOfAsteroids; i++) {

			int xLocationInMap = Utilities.randomInt(0, this.width);
			int yLocationInMap = Utilities.randomInt(0, this.height);
			
			xLocationInMap = notNearCenter(xLocationInMap);
			yLocationInMap = notNearCenter(yLocationInMap);
			
			double xVelocity = Utilities.randomDouble(1.0, 4.0);
			double yVelocity = Utilities.randomDouble(1.0, 4.0);
			
			asteroids.add(new Asteroid(xLocationInMap, yLocationInMap, xVelocity, yVelocity));
		}
	}

	int notNearCenter(int coordinate){
		if((coordinate/2 - WIDTH /3) < 0){
			return 0;
		}
		
		return coordinate;
	}
	public void addMissile(Missile m){
		this.missiles.add(m);
	}
	
	int getWidth() {
		return this.width;
	}
	
	int getHeight() {
		return this.height;
	}
	
	Player getPlayer() {
		return this.player;
	}
	
    private void drawBackground() {
        imageMode(CENTER);
        this.backgroundOffsetX = (this.backgroundOffsetX+1)%this.width;
        image(this.backgroundImage, this.width/2 - this.backgroundOffsetX, this.height/2);
        image(this.backgroundImage, this.width/2 + this.width - this.backgroundOffsetX, this.height/2);
    }
    
	void update() {
        //this.drawBackground();
        
        bg.draw();
        
        // Update player
        // Condition: game isn't over
        if (!gameOver)
            this.player.update();
        
        // Update asteroids
        for (asteroids s: asteroids) {
            s.update();
        }
        
        // Update missiles
        for (missile m: missiles) {
            if (m.ttl < 0) {
                this.missiles.remove(m);
                continue;
            }
            m.update();
        }
        
        // Check for collisions between asteroids and missiles
        for (asteroid s: asteroids) {
            for (missile m: missiles) {
                if (m.collide(s)) {
                    this.scheduler.add(s.die());
                    this.player.addScore(SCORE_PER_ASTEROID);
                    this.asteroids.remove(s);
                    this.missiles.remove(m);
                    continue;
                }
            }
        }
        
        // Check for collisions between asteroids and the player
        // Condition: game isn't over
        if (!this.gameOver) {
            for (asteroid s: asteroids) {
                if (player.collide(s)) {
                    this.gameOver = true;
                    this.scheduler.add(s.die());
                    this.scheduler.add(this.player.die());
                    this.asteroids.remove(s);
                }
            }
        }
        
        // Print the information about player score and current wave
        this.printInfoText();
        
        // Start a new game if the player has died and all the scheduled
        // animations are over
        if (this.gameOver && this.scheduler.size() == 0) {
            justDied = FRAME_RATE;
            startNewGame();
        }
        
        for (element e: scheduler) {
            if (e.isFinished())
                this.scheduler.remove(e);
            else
                e.draw();
        }
        
        this.checkIfLevelCleared();
        if (this.nextWaveComing > 0) {
            this.printWaveComing();
        }
	}

	private void printWaveComing() {        
        int sec = int((this.nextWaveComing-millis())/1000);
        
        if (sec < 2)
            fill(255, 0, 0, 75);
        else
            fill(255, 255, 255, 75);
            
        textSize(50);    
		textAlign(CENTER);
		text("Next wave in " + sec + " seconds", this.width/2, this.height/2);
	}
	
    private void printInfoText() {
        textSize(15);
		fill(255, 255, 255, 75);
		textAlign(LEFT);
		text("Score: " + this.player.getScore(), 15, 23);
        text("Wave: " + this.currentWave, 15, 50);
        text("Asteroids left: " + this.asteroids.size(), 15, 78);
    }
    
    public void printPausedText() {
        textSize(65);
        fill(255, 255, 255, 80);
		textAlign(CENTER);
		text("Paused", this.width/2, this.height/2);
    }
    
	void startNewGame() {
		startScreen = true;
		w = new StartScreen(WIDTH, HEIGHT, 10);
	}
	
	private void checkIfLevelCleared() {
        if (!this instanceof StartScreen) {
        
            if (this.asteroids.size() == 0 && this.nextWaveComing == 0) {
                this.nextWaveComing = millis() + WAVE_DELAY;
            }
            
            if (this.nextWaveComing < millis() && this.asteroids.size() == 0) {
                this.nextWaveComing = 0;
                this.currentWave++;
                this.addAsteroids(this.currentWave*5);
            }
        }
    }
}
	


//// START SCREEN /////////////////

public class Screen {

    private int width;
	private int height;
    private PImage backgroundImage;

    Screen(int width, int height) {
        this.width = width;
        this.height = height;
        this.backgroundImage = loadImage(BACKGROUND_IMAGE);
    }
    
    Screen(int width, int height, String backgroundImageFile) {
        this.width = width;
        this.height = height;
        this.backgroundImage = loadImage(backgroundImageFile);
    }
    
    abstract void draw();
}

public class BetterStartScreen extends Screen {

    private int blinkingTextCounter = 0;
    private int willBlink = true;
    
    BetterStartScreen(int width, int height) {
        super(width, height);
    }
    
    BetterStartScreen(int width, int height, String backgroundImageFile) {
        super(width, height, backgroundImageFile);
    }
    
    public void draw() {
        this.printGreetingsText();
    }
    
    private void printGreetingsText() {
        // Print the A!STEROIDS logo
        textSize(64);
        textAlign(LEFT);
        
		fill(255, 255, 255);
        text("A", (this.width/2)-190 + 0, 150);
        fill(255, 255, 0);
		text("!", (this.width/2)-190 + 40, 150);
        fill(255, 255, 255);
        text("STEROIDS", (this.width/2)-190 + 60, 150);
        
        // Print the "PRESS SPACE TO START"
		textSize(24);
		textAlign(CENTER);
		this.blinkingTextCounter++;
		if (this.blinkingTextCounter > FRAME_RATE) {
			this.blinkingTextCounter = 0;
			this.willBlink = !this.willBlink;
		}
		if (this.willBlink) {
			fill(255, 0, 0);
		}
		text("PRESS SPACE TO START", WIDTH/2, HEIGHT - 200);
	}
    
}

public class StartScreen extends World {
	
	int blinkingTextCounter = 0;
	int willBlink = true;
	
	public StartScreen(int width, int height, int a){
		super(width, height, a);
	}
	
	void update(){

		super.update();
		if(justDied !=0 ){
			justDied--;
		}
		this.printGreetingsText();
		
	}
	
	void printGreetingsText() {
        // Print the A!STEROIDS logo
        textSize(64);
        textAlign(LEFT);
        
		fill(255, 255, 255);
        text("A", (this.width/2)-190 + 0, 150);
        fill(255, 255, 0);
		text("!", (this.width/2)-190 + 40, 150);
        fill(255, 255, 255);
        text("STEROIDS", (this.width/2)-190 + 60, 150);
        
        // Print the "PRESS SPACE TO START"
		textSize(24);
		textAlign(CENTER);
		blinkingTextCounter++;
		if (blinkingTextCounter > FRAME_RATE) {
			blinkingTextCounter = 0;
			willBlink = !willBlink;
		}
		if(willBlink){
			fill(255, 0, 0);
		}
		text("PRESS SPACE TO START", WIDTH/2, HEIGHT - 200);
	}
	
	Player getPlayer() {
		return new Player(){
			
	
			public void update (){
			}

		};
	}
	

}



///////////////// GLOBALS /////////////////////

boolean PAUSED = false;
boolean startScreen = true;
int justDied = 0;

World w;

final static boolean DEBUG = false;

final static int WIDTH = 800;
final static int HEIGHT = 600;

final static int MISSILE_COOLDOWN = 10;
final static int MISSILE_TIME_TO_LIVE = 60;
final static int NUMBER_OF_ASTEROIDS = 5;
final static int ASTEROID_SIZE = 30;
final static int FRAME_RATE = 60;
final static int SPRITE_COOLDOWN = FRAME_RATE/3;

final static int WAVE_DELAY = 6*1000; // Millieconds
final static int SCORE_PER_ASTEROID = 8;

final static double DAMPENING = 0.995;
final static double ACCELERATION = 0.2;
final static double ANGULAR_ACCELERATION = 0.1;
final static double PLAYER_MAX_VELOCITY = 8.0;
final static double ASTEROID_MAX_VELOCITY = 10;
final static double MISSILE_VELOCITY = 7.0;

final static String BACKGROUND_IMAGE = "images/starfield_background.jpg";


///////////////// SETUP & MAIN LOOP /////////////////////

void setup() {
    colorMode(RGB, 255, 255, 255, 100);

	w = new StartScreen(WIDTH, HEIGHT, 10);
    
    size(w.getWidth(), w.getHeight());
    background(125);
    fill(255);
    frameRate(FRAME_RATE);
    
    //PFont fontA = loadFont("courier");
    textFont(createFont("helveticaneue-webfont", 18));
    
    soundEffects["background_music"].play();
}

void draw() {
    handleKeys();
    if (!PAUSED)
        w.update();
    else
        w.printPausedText();
}


///////////////// KEYBOARD HANDLING /////////////////////

boolean[] pressedKeys = new boolean[256];

void keyPressed() {
    Player p = w.getPlayer();

    switch (keyCode) {
        // Turning right
        case 37:
            p.setTurningLeft();
            break;
        case 38:
            p.setAccelerating();
            break;
        case 39:
            p.setTurningRight();
            break;
        case 80:
            PAUSED = !PAUSED;
            break;
    }

	pressedKeys[keyCode] = true;
}

void keyReleased() {
    Player p = w.getPlayer();

    switch (keyCode) {
        // Turning right
        case 37:
            p.unsetTurningLeft();
            break;
        case 38:
            p.unsetAccelerating();
            break;
        case 39:
            p.unsetTurningRight();
            break;
    }

    pressedKeys[keyCode] = false;
}

void handleKeys() {
	
	if(justDied != 0){
		return;
	}
    
	if (startScreen) {
	    if (pressedKeys[32]) {
	    	startScreen = false;
	        w = new World(WIDTH, HEIGHT, 5);
	    }
	}
	
    else {
	    
	    Player p = w.getPlayer();
	    // Shoot
	    if (pressedKeys[32]) {
	        p.shoot();
	    }
	    
	    // Rotate left
	    if (pressedKeys[37]) {
	        p.angle -= ANGULAR_ACCELERATION;
	        if (p.angle > 2.0*TWO_PI) {
	            p.angle = 0.0;
	        }
	        if (p.angle < 0.0) {
	            p.angle += 2.0*TWO_PI;
	        }
	    }
	    
	    // Positive y
	    if (pressedKeys[38]) {
	        double ovy = p.vy, ovx = p.vx;
	    
	        p.vy += ACCELERATION * Math.sin(p.angle);
	        p.vx += ACCELERATION * Math.cos(p.angle);
	        
	        double total = p.getTotalVelocity();
	        if (total > PLAYER_MAX_VELOCITY) {
	            p.vy = ovy;
	            p.vx = ovx;
	        }
	    }
	    
	    // Rotate right
	    if (pressedKeys[39]) {
	        p.angle += ANGULAR_ACCELERATION;
	        if (p.angle > 2.0*TWO_PI) {
	            p.angle = 0.0;
	        }
	        if (p.angle < 0.0) {
	            p.angle += 2.0*TWO_PI;
	        }
	    }
	    
	    // Negative y
	    if (pressedKeys[40]) {
	        double ovy = p.vy, ovx = p.vx;
	    
	        p.vy -= ACCELERATION * Math.sin(p.angle);
	        p.vx -= ACCELERATION * Math.cos(p.angle);
	        
	        double total = p.getTotalVelocity();
	        if (total > PLAYER_MAX_VELOCITY) {
	            p.vy = ovy;
	            p.vx = ovx;
	        }
	    }
	}
	
}




/////////// REMOVE ME ///////////////
public class ProceduralBackground {
    // the twinlking star locations
    private int[] starX = new int[100];
    private int[] starY = new int[100];
    private color[] starColor = new color[100];
    private int starSize = 3; // the size of the twinkling stars

    // the tail of the shooting star
    private int[] shootX = new int[30];
    private int[] shootY = new int[30];
    private int METEOR_SIZE = 10; // initial size when it first appears
    private float meteorSize = METEOR_SIZE; // size as it fades

    // distance a shooting star moves each frame - varies with each new shooting star
    private float ssDeltaX, ssDeltaY; 
    // -1 indicates no shooting star, this is used to fade out the star
    private int ssTimer = -1;
    // starting point of a new shooting star, picked randomly
    private int startX, startY;

    private int width, height;

    ProceduralBackground(int width, int height) {
        this.width = width;
        this.height = height;
        for (int i = 0; i < starX.length; i++) {
            starX[i] =(int)random(width);
            starY[i] = (int)random(height);
            starColor[i] = color((int)random(100,255));
        }
    }
    
    public void draw() {
        background(0,0,50); // dark blue night sky
  
        // Draw the stars
        stroke(0);
        strokeWeight(1);
        for (int i = 0; i < starX.length; i++) {
            fill(random(50,255)); // makes them twinkle
            if (random(10) < 1) {
                starColor[i] = (int)random(100,255);
            }
            fill(starColor[i]);
            ellipse(starX[i], starY[i], starSize, starSize);
        }

        // Draw the shooting stars
        for (int i = 0; i < shootX.length-1; i++) {
            int shooterSize = max(0,int(meteorSize*i/shootX.length));
            // To get the tail to disappear need to switch to noStroke when it gets to 0
            if (shooterSize > 0) {
                strokeWeight(shooterSize);
                stroke(255);
            }
            else
                noStroke();
            line(shootX[i], shootY[i], shootX[i+1], shootY[i+1]);
        }
        meteorSize*=0.9; // Shrink the shooting star as it fades

        // Move the shooting star along it's path
        for (int i = 0; i < shootX.length-1; i++) {
            shootX[i] = shootX[i+1];
            shootY[i] = shootY[i+1];
        }

        // Add the new points into the shooting star as long as it hasn't burnt out
        if (ssTimer >= 0 && ssTimer < shootX.length) {
            shootX[shootX.length-1] = int(startX + ssDeltaX*(ssTimer));
            shootY[shootY.length-1] = int(startY + ssDeltaY*(ssTimer));
            ssTimer++;
            if (ssTimer >= shootX.length) {
                ssTimer = -1; // end the shooting star
            }
        }

        // Create a new shooting star with some random probability
        if (random(100) < 1 && ssTimer == -1) {
            newShootingStar();
        }
    }
    
    /*
      Starts a new shooting star by randomly picking start and end point.
    */
    private void newShootingStar() {
        int endX, endY;
        startX = (int)random(width);
        startY = (int)random(height);
        endX = (int)random(width);
        endY = (int)random(height);
        ssDeltaX = (endX - startX)/(float)(shootX.length);
        ssDeltaY = (endY - startY)/(float)(shootY.length);
        ssTimer = 0; // starts the timer which ends when it reaches shootX.length
        meteorSize = METEOR_SIZE;
        // by filling the array with the start point all lines will essentially form a point initialy
        for (int i = 0; i < shootX.length; i++) {
            shootX[i] = startX;
            shootY[i] = startY;
        }
    }
    
}