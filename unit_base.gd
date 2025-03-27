extends RigidBody2D


#<-----------------------------Properties of entity---------------------------->

@export var max_healt		:int		= 1000

@export var healt			:int		= max_healt

@export var defense			:int		= 0

@export var speed			:int		= 1

@export var jump_velocity	:int		= 1

@export var click_damage	:int		= 200

@export var objective 		:Vector2	= Vector2(740,505)

@export var is_enemy		:bool		= false

@export var is_death		:bool		= false

@export var max_rotation	:int		= 90

@export var act_rotation	:int		= 0

#<---extra--->
@export var max_force_x		:int		= 60

@export var min_force_x		:int		= 20

@export var max_force_y		:int		= -80 #Negative to make it go up

@export var min_force_y		:int		= -100 

@export var velocity		:Vector2	= Vector2.ZERO

var force_x					:float		= 0.0

var force_y					:float		= 0.0

var gravity_scale_custom	:float		= 0

#<----------------------------Child Node References---------------------------->

@onready var sprite			= $SpriteEntityBase
@onready var coll_shape		= $CollisionShapeEntityBase
@onready var anim_player	= $AnimationPlayerEntityBase

#<----------------------------------Functions---------------------------------->

func _ready():
	$HealtBar.hide()
	self.healt = self.max_healt
	is_enemy = self.is_in_group("Enemy")
	self.lock_rotation = true

func _physics_process(delta):
	move()

func move():
	if !is_death:
		self.gravity_scale = gravity_scale_custom
		if self.position.distance_to(objective) > 1:
			self.velocity = position.direction_to(objective) * speed
			move_and_collide(self.velocity)
		else:
			#self.queue_free()
			pass#print("Damage recived")
			#self.objective = Vector2(600,10)
	else:
		if self.is_in_group("Fly"):
			act_rotation +=1
			self.rotation_degrees = act_rotation
		self.gravity_scale += 0.01

func die():
	is_death = true
	self.linear_velocity = Vector2.ZERO
	self.lock_rotation = false
	self.gravity_scale += 0
	apply_custom_force()
	self.rotation_degrees = act_rotation
	self.set_collision_layer_value(4,false)
	$ClickArea.visible = false
	if self.is_in_group("Fly"):
			$Timer.wait_time = 2
	$Timer.start()
	$HealtBar.hide()
	
	
func damage_manager(damage): #Recibe damage from source and aply it to healt and if reach 0 delete the entity
	self.healt -= damage
	healtbar_manager()
	if self.healt <= 0:
			die()

func healtbar_manager(): #Update the healtbar value to show the healt change
	$HealtBar.show()
	$HealtBar.value = healt

func apply_custom_force():
	force_x = randf_range(min_force_x, max_force_x)
	force_y = randf_range(min_force_y, max_force_y)
	apply_central_impulse(Vector2(force_x,force_y))

func _on_hurt_box_area_entered(AreaEnter):
		if AreaEnter.is_in_group("Hitbox"):
			damage_manager(AreaEnter.damage)


func _on_timer_timeout():
	queue_free()


func _on_click_area_input_event(viewport, event, shape_idx):
	if  event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		if is_enemy:
			damage_manager(self.click_damage)
