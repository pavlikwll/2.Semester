=== Fisherman ===

= start
Fisherman: The water was looking at me again today.

Bartek: Water does not look at people.

Fisherman: That's exactly what it wants you to think.

Fisherman: It has also taken my lucky fish.

Bartek: The water stole your fish?

Fisherman: No. Kopatych did.

Bartek: The giant mushroom in town?

Fisherman: He borrowed her without asking.

Fisherman: Then he came back alone and said she had chosen freedom.

Bartek: And you believed him?

Fisherman: I believe everyone once.

Fisherman: It makes betrayal easier to organize.

* [Where can I find Kopatych?] -> find_kopatych
* [What is so special about this fish?] -> special_fish
* [This sounds like a personal problem.] -> personal_problem


= find_kopatych
Fisherman: In the middle of town.

Fisherman: Large mushroom. Suspicious confidence. Smells like soup and bad decisions.

Bartek: That description is disturbingly specific.

Fisherman: Ask him what he did with my fish.

Fisherman: And do not let him change the subject to chemistry.

Bartek: Why would he do that?

Fisherman: Because crime sounds more respectable when he calls it chemistry.

-> END


= special_fish
Fisherman: She is a swamp lantern carp.

Fisherman: Green in daylight. Golden beneath the moon.

Fisherman: She brought me luck for seven years.

Bartek: What kind of luck?

Fisherman: I have not drowned.

Bartek: That is a very low standard.

Fisherman: The lake is ambitious.

Fisherman: Find Kopatych. He knows where she is.

-> END


= personal_problem
Fisherman: It was.

Fisherman: Then you started asking questions.

Bartek: That does not make it my problem.

Fisherman: Kopatych lives directly on the road you are walking toward.

Fisherman: Destiny is very lazy around here.

Bartek: Fine. I will speak to him.

Fisherman: Good.

Fisherman: Tell him I am still emotionally armed.

-> END


= waiting_for_fish
Fisherman: Did Kopatych tell you where she is?

Bartek: He released her near the old pier.

Fisherman: Of course he did.

Fisherman: He thinks freedom means creating problems for other people.

Bartek: I still have to catch her.

Fisherman: Then take a rod and be patient.

Fisherman: Lucky fish do not bite for people in a hurry.

-> END


= green_fish
Bartek: I found your fish.

Fisherman: ...

Fisherman: Oh.

Bartek: That sounded almost normal.

Fisherman: She remembers me.

Bartek: How can you tell?

Fisherman: She is judging my posture.

Bartek: Are you taking her back?

Fisherman: No.

Fisherman: If she chose the lake, the lake may keep her.

Bartek: So I caught her for nothing?

Fisherman: Not for nothing.

Fisherman: You brought back something I thought was gone.

~ Event("remove_green_fish")
~ Event("give_fisherman_token")

Fisherman: Take this.

Bartek: A metal fish?

Fisherman: A Lucky Fish Token.

Fisherman: The old lakekeepers carried them before the caves were sealed.

Bartek: What am I supposed to do with it?

Fisherman: Someone below Kopatych's house may still recognize it.

Bartek: You could have mentioned the person living under his house earlier.

Fisherman: You had not earned the confusing part yet.

~ Event("start_fisherman_patrol")

-> END


= after_fish
Fisherman: She returned to the lake.

Bartek: And you are just leaving?

Fisherman: I have been sitting here for seven years.

Fisherman: My legs have started developing political opinions.

Bartek: Enjoy your walk.

Fisherman: Enjoy the basement.

-> END