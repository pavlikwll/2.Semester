=== Guard ===

= start
Guard: Stop.

Bartek: I was already standing still.

Guard: Then continue doing so.

Bartek: I need to enter the cave.

Guard: No one enters the lower lake.

Bartek: Kopatych gave me the key.

Guard: Kopatych once gave a key to a chicken.

Guard: The chicken was more convincing.

Bartek: What do you want?

Guard: Proof that the lake has accepted you.

* [Show the Lucky Fish Token.] -> show_token
* [Try to threaten him.] -> threaten
* [Leave.] -> leave


= show_token
Bartek: The fisherman gave me this.

Guard: A Lucky Fish Token.

Bartek: You recognize it?

Guard: It belonged to the old lakekeepers.

Guard: Only someone who returned what was lost may carry one.

Bartek: So I can pass?

Guard: The token may pass.

Bartek: I am holding the token.

Guard: An unfortunate technicality.

~ Event("remove_fisherman_token")
~ Event("open_cave_passage")

Guard: Go.

Guard: The lower lake remembers every visitor.

Bartek: Does anything around here behave normally?

Guard: No.

-> END


= threaten
Bartek: Move, or I will make you move.

Guard: I have guarded this door for thirty-two years.

Guard: I have survived floods, spores, slimes and Kopatych's cooking.

Guard: You are currently fifth on my list of concerns.

Bartek: What is first?

Guard: Moisture.

Bartek: Fine. I will find another way.

Guard: Bring proof from the lakekeeper.

-> END


= leave
Bartek: I will come back.

Guard: Most people say that.

Bartek: Do they?

Guard: No.

-> END


= after_open
Guard: The passage is open.

Bartek: Any advice?

Guard: Do not follow voices coming from beneath the water.

Bartek: What if they know the way?

Guard: Then they do not need your help.

-> END