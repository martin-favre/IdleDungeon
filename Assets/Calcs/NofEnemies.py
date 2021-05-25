import random
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D

def calcDmgSlowKilling(nOfEnemies, hpPerEnemy):
    """ If you kill enemies the slowest way possible 
        by distributing dmg among them.
    """
    return nOfEnemies*nOfEnemies*hpPerEnemy

def calcDmgFastKilling(nOfEnemies, hpPerEnemy):
    """ If you kill enemies the quickest way possible 
        by focusing one by one
    """
    return nOfEnemies*hpPerEnemy

def plt3d():
    x = range(1, 10)
    y = range(1, 100)
    points3d = [(xs, ys, calcDmgSlowKilling(xs, ys)) for ys in y for xs in x]

def plt2d():
    nofEnemies = range(1, 5)
    x = range(1, 1000)
    ys = []
    for nofEnemy in nofEnemies:
        ys.append([])
        for hp in x: 
            dmg = calcDmgSlowKilling(nofEnemy, hp)
            ys[-1].append(dmg)
    for index, y in enumerate(ys):
        plt.plot(x, y, label=nofEnemies[index])
#plt2d()
print(calcDmgSlowKilling(4, 30))
print(calcDmgSlowKilling(1, 400))
#z1 = [calcDmg(nof, 10) for nof in x]
#z2 = [calcDmg(nof, 20) for nof in x]
#z3 = [calcDmg(nof, 30) for nof in x]
#z4 = [calcDmg(nof, 40) for nof in x]
#fig = plt.figure()
#ax = fig.add_subplot(111, projection='3d')
#[ax.scatter(p[0], p[1], p[2]) for p in points]
#ax.scatter(x, 10, z1)

#[plt.plot(p[1], p[2], label=p[0]) for p in points]
#plt.plot(x, y1, zs=10, label='10')
#plt.plot(x, y2, zs=20, label='20')
#plt.plot(x, y3, zs=30, label='30')
#plt.plot(x, y4, zs=40, label='40')
#plt.grid()
#plt.xlabel('x - nof enemies')
#plt.ylabel('y - dmg per enemy')
#plt.zlabel('z - total dmg')
#plt.yscale('log')
#plt.legend()
#plt.show()
