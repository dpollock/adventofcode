original source: [https://adventofcode.com/2022/day/20](https://adventofcode.com/2022/day/20)
## --- Day 20: Grove Positioning System ---
It's finally time to meet back up with the Elves. When you try to contact them, however, you get no reply. Perhaps you're out of range?

You know they're headed to the grove where the <em>star</em> fruit grows, so if you can figure out where that is, you should be able to meet back up with them.

Fortunately, your handheld device has a file (your puzzle input) that contains the grove's coordinates! Unfortunately, the file is <em>encrypted</em> - just in case the device were to fall into the wrong hands.

Maybe you can decrypt it?

When you were still back at the camp, you overheard some Elves talking about coordinate file encryption. The main operation involved in decrypting the file is called <em>mixing</em>.

The encrypted file is a list of numbers. To <em>mix</em> the file, move each number forward or backward in the file a number of positions equal to the value of the number being moved. The list is <em>circular</em>, so moving a number off one end of the list wraps back around to the other end as if the ends were connected.

For example, to move the <code>1</code> in a sequence like <code>4, 5, 6, <em>1</em>, 7, 8, 9</code>, the <code>1</code> moves one position forward: <code>4, 5, 6, 7, <em>1</em>, 8, 9</code>. To move the <code>-2</code> in a sequence like <code>4, <em>-2</em>, 5, 6, 7, 8, 9</code>, the <code>-2</code> moves two positions backward, wrapping around: <code>4, 5, 6, 7, 8, <em>-2</em>, 9</code>.

The numbers should be moved <em>in the order they originally appear</em> in the encrypted file. Numbers moving around during the mixing process do not change the order in which the numbers are moved.

Consider this encrypted file:

<pre>
<code>1
2
-3
3
-2
0
4
</code>
</pre>

Mixing this file proceeds as follows:

<pre>
<code>Initial arrangement:
1, 2, -3, 3, -2, 0, 4

1 moves between 2 and -3:
2, 1, -3, 3, -2, 0, 4

2 moves between -3 and 3:
1, -3, 2, 3, -2, 0, 4

-3 moves between -2 and 0:
1, 2, 3, -2, -3, 0, 4

3 moves between 0 and 4:
1, 2, -2, -3, 0, 3, 4

-2 moves between 4 and 1:
1, 2, -3, 0, 3, 4, -2

0 does not move:
1, 2, -3, 0, 3, 4, -2

4 moves between -3 and 0:
1, 2, -3, 4, 0, 3, -2
</code>
</pre>

Then, the grove coordinates can be found by looking at the 1000th, 2000th, and 3000th numbers after the value <code>0</code>, wrapping around the list as necessary. In the above example, the 1000th number after <code>0</code> is <code><em>4</em></code>, the 2000th is <code><em>-3</em></code>, and the 3000th is <code><em>2</em></code>; adding these together produces <code><em>3</em></code>.

Mix your encrypted file exactly once. <em>What is the sum of the three numbers that form the grove coordinates?</em>


