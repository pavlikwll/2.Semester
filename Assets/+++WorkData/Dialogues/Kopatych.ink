=== Kopatych ===

= start
# avatar:kopatych
Kopatych: Hey there, sexy traveler.

Bartek: ...Did a mushroom just flirt with me?

# avatar:kopatych
Kopatych: Honey, after three rainy nights and a suspicious amount of spores, anything can happen.

Bartek: The fisherman says you stole his lucky fish.

# avatar:kopatych
Kopatych: Stole is such an emotionally expensive word.

Bartek: Did you take it?

# avatar:kopatych
Kopatych: Temporarily.

Bartek: Where is it now?

# avatar:kopatych
Kopatych: Living its best life near the old pier.

Bartek: You released it?

# avatar:kopatych
Kopatych: It looked at me with the eyes of a creature that had seen taxation.

* [Why did you take the fish?] -> fish_reason
* [How do I get into your house?] -> house
* [You are clearly insane.] -> insane


= fish_reason
# avatar:kopatych
Kopatych: I needed it for an experiment.

Bartek: What kind of experiment?

# avatar:kopatych
Kopatych: The kind that becomes illegal only after someone writes it down.

Bartek: And then you released it.

# avatar:kopatych
Kopatych: The fish objected to the methodology.

Bartek: I am going to catch it.

# avatar:kopatych
Kopatych: Use the pier outside town.

-> quest_offer


= house
Bartek: I need to reach the caves beneath your house.

# avatar:kopatych
Kopatych: Straight to the basement? Bold.

Bartek: Can you give me the key?

# avatar:kopatych
Kopatych: I can.

Bartek: Will you?

# avatar:kopatych
Kopatych: That depends on your relationship with radioactive mushrooms.

-> quest_offer


= insane
# avatar:kopatych
Kopatych: Correct.

Bartek: At least you admit it.

# avatar:kopatych
Kopatych: Honesty is cheaper than therapy.

Bartek: Tell me how to reach the caves.

# avatar:kopatych
Kopatych: Through the basement of my house.

Bartek: And the key?

# avatar:kopatych
Kopatych: We are getting there, sugarcap.

-> quest_offer


= quest_offer
# avatar:kopatych
Kopatych: I need four radioactive mushrooms from the glowing grove.

Bartek: Why?

# avatar:kopatych
Kopatych: Science. Crime. Dinner. Depends who asks.

Bartek: And if I bring them?

# avatar:kopatych
Kopatych: I give you the key to my house.

Bartek: The house with the cave entrance in the basement?

# avatar:kopatych
Kopatych: Exactly.

Bartek: Why is there a cave entrance under your house?

# avatar:kopatych
Kopatych: The rent was suspiciously low.

Bartek: Fine. Four radioactive mushrooms.

# avatar:kopatych
Kopatych: Bring them back glowing.

-> END


= waiting_for_mushrooms
# avatar:kopatych
Kopatych: Four radioactive mushrooms.

Bartek: I remember.

# avatar:kopatych
Kopatych: Wonderful. Short-term memory is very attractive.

Bartek: I am leaving now.

# avatar:kopatych
Kopatych: Try not to become fluorescent.

-> END


= deliver_mushrooms
Bartek: I brought your four radioactive mushrooms.

# avatar:kopatych
Kopatych: Oh, gorgeous.

# avatar:kopatych
Kopatych: They are glowing with emotional instability.

Bartek: Same.

~ Event("remove_radioactive_mushrooms")
~ Event("give_house_key")
~ Event("enable_house_teleport")

# avatar:kopatych
Kopatych: As promised, here is the key to my house.

Bartek: And the basement leads into the caves?

# avatar:kopatych
Kopatych: Eventually.

Bartek: What does that mean?

# avatar:kopatych
Kopatych: There is a guard below.

Bartek: You have a guard in your basement?

# avatar:kopatych
Kopatych: Technically, the guard has me above his cave.

Bartek: Will he let me through?

# avatar:kopatych
Kopatych: Not unless you bring something he respects.

Bartek: Such as?

# avatar:kopatych
Kopatych: Ask the fisherman.

Bartek: You could have told me this before.

# avatar:kopatych
Kopatych: Then our relationship would have developed too quickly.

-> END


= after_key
# avatar:kopatych
Kopatych: The house is open.

Bartek: And the guard is waiting below.

# avatar:kopatych
Kopatych: He is always waiting.

Bartek: For what?

# avatar:kopatych
Kopatych: Nobody knows.

# avatar:kopatych
Kopatych: That is what makes him professional.

-> END